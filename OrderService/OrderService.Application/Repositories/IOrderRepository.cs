using OrderService.Application.Models;

namespace OrderService.Application.Repositories;

public interface IOrderRepository
{
    public void CreateOrder(Order order);
    public Order? GetOrderById(Guid id);
    public void UpdateOrderStatus(Guid id, string status);
}
