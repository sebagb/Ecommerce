namespace ProductService.Application.Models;

public class Product
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Category { get; set; }
    public required decimal Price { get; set; }
    public DateOnly ExpirationDate { get; set; }
    public string? Provider { get; set; }
    public int Stock { get; set; }
    public string? SellingSeason { get; set; }
}