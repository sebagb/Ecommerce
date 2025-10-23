using System.Text;
using RabbitMQ.Client;

namespace OrderService.Application.MessageQueueing;

public class OrderCreatedProducer(
    string hostName,
    string queueName)
{
    private readonly string hostname = hostName;
    private readonly string queueName = queueName;

    public async Task Publish(Guid orderId)
    {
        var factory = new ConnectionFactory { HostName = hostname };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var body = Encoding.UTF8.GetBytes(orderId.ToString());

        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: queueName,
            body: body
        );
    }
}