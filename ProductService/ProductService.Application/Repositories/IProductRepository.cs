using ProductService.Application.Models;

namespace ProductService.Application.Repositories;

public interface IProductRepository
{
    public void Create(Product product);
    public Product GetById(Guid id);
    public void Update(Product product);
}