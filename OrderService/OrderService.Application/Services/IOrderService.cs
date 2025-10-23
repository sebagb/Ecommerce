using OrderService.Application.Models;

namespace OrderService.Application.Services;

public interface IOrderService
{
    public void CreateOrder(Order order);
    public Order? GetOrderById(Guid id);
    public void UpdateOrderStatus(Guid id, string status);
}