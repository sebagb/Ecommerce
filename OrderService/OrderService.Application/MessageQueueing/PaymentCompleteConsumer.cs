using System.Text;
using Microsoft.Extensions.Hosting;
using OrderService.Application.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OrderService.Application.MessageQueueing;

public class PaymentCompleteConsumer
    (string hostName,
     string queueName,
     IOrderService service)
    : IHostedService
{
    private readonly string hostName = hostName;
    private readonly string queueName = queueName;
    private readonly IOrderService service = service;
    private IConnection? connection;
    private IChannel? channel;
    private const string delimiter = ":";
    private const int orderIdPosition = 0;
    private const int statusPosition = 1;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory { HostName = hostName };

        connection = await factory
            .CreateConnectionAsync(cancellationToken);
        channel = await connection
            .CreateChannelAsync(cancellationToken: cancellationToken);

        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            var result = message.Split(delimiter);
            var id = new Guid(result[orderIdPosition]);
            var status = result[statusPosition];
            service.UpdateOrderStatus(id, status);

            return Task.CompletedTask;
        };

        await channel.BasicConsumeAsync(
            queue: queueName,
            autoAck: true,
            consumer: consumer,
            cancellationToken: cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (channel != null)
        {
            await channel.CloseAsync();
            await channel.DisposeAsync();
        }

        if (connection != null)
        {
            await connection.CloseAsync();
            await connection.DisposeAsync();
        }
    }
}