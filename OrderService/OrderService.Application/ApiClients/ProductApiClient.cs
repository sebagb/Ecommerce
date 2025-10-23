using System.Text;
using System.Text.Json;
using ProductService.Contract.Responses;
using ProductService.Contract.Requests;

namespace OrderService.Application.ApiClients;

public class ProductApiClient(HttpClient httpClient)
{
    private readonly HttpClient httpClient = httpClient;

    public ProductResponse GetProductById(Guid productId)
    {
        var response = httpClient.GetAsync(productId.ToString()).Result;
        var json = response.Content.ReadAsStringAsync().Result;
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        return JsonSerializer.Deserialize<ProductResponse>(json, options)!;
    }

    public void UpdateProduct(ProductResponse product)
    {
        var request = new UpdateProductRequest
        {
            Name = product.Name,
            Category = product.Category,
            Price = product.Price,
            ExpirationDate = product.ExpirationDate,
            Provider = product.Provider,
            SellingSeason = product.SellingSeason,
            Stock = product.Stock,
        };

        using StringContent jsonContent = new(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        var response = httpClient.PutAsync(
            product.Id.ToString(),
            jsonContent)
            .Result;
    }
}