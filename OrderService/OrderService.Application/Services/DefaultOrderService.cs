using OrderService.Application.ApiClients;
using OrderService.Application.MessageQueueing;
using OrderService.Application.Models;
using OrderService.Application.Repositories;

namespace OrderService.Application.Services;

public class DefaultOrderService
    (IOrderRepository repository,
    OrderCreatedProducer producer,
    UserApiClient userApiClient,
    ProductApiClient productApiClient)
    : IOrderService
{
    private readonly IOrderRepository repository = repository;
    private readonly OrderCreatedProducer producer = producer;
    private readonly UserApiClient userApiClient = userApiClient;
    private readonly ProductApiClient productApiClient = productApiClient;
    private const string paymentSucceeded = "Succeeded";
    private const string paymentFailed = "Failed";

    public void CreateOrder(Order order)
    {
        var user = userApiClient.GetUserById(order.CustomerId);
        var isValidCustomer =
            user != null
            && !string.IsNullOrEmpty(user.Email);

        var product = productApiClient.GetProductById(order.ProductId);
        var enoughStock = order.Quantity <= product.Stock;
        if (enoughStock)
        {
            var updatedStock = product.Stock - order.Quantity;
            product.Stock = updatedStock;
            productApiClient.UpdateProduct(product);
            producer.Publish(order.OrderId).Wait();

            repository.CreateOrder(order);
        }
    }

    public Order? GetOrderById(Guid id)
    {
        return repository.GetOrderById(id);
    }

    public void UpdateOrderStatus(Guid id, string status)
    {
        if (status.Equals(paymentFailed))
        {
            var order = repository.GetOrderById(id);
            var product = productApiClient.GetProductById(order.ProductId);

            var updatedStock = product.Stock + order.Quantity;
            product.Stock = updatedStock;
            productApiClient.UpdateProduct(product);

        }

        repository.UpdateOrderStatus(id, status);
    }
}