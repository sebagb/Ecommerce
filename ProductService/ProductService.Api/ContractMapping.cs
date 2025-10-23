using ProductService.Application.Models;
using ProductService.Contract.Requests;
using ProductService.Contract.Responses;

namespace ProductService.Api;

public static class ContractMapping
{
    public static Product MapToProduct(this CreateProductRequest request)
    {
        return new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Category = request.Category,
            Price = request.Price,
            ExpirationDate = request.ExpirationDate,
            Provider = request.Provider,
            Stock = request.Stock,
            SellingSeason = request.SellingSeason
        };
    }

    public static Product MapToProduct(
        this UpdateProductRequest request,
        Guid id)
    {
        return new Product
        {
            Id = id,
            Name = request.Name,
            Category = request.Category,
            Price = request.Price,
            ExpirationDate = request.ExpirationDate,
            Provider = request.Provider,
            Stock = request.Stock,
            SellingSeason = request.SellingSeason
        };
    }

    public static ProductResponse MapToResponse(this Product product)
    {
        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Category = product.Category,
            Price = product.Price,
            ExpirationDate = product.ExpirationDate,
            Provider = product.Provider,
            Stock = product.Stock,
            SellingSeason = product.SellingSeason
        };
    }
}