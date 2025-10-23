namespace ProductService.Contract.Requests;

public class CreateProductRequest
{
    public required string Name { get; set; }
    public required string Category { get; set; }
    public required decimal Price { get; set; }
    public DateOnly ExpirationDate { get; set; }
    public string? Provider { get; set; }
    public int Stock { get; set; }
    public string? SellingSeason { get; set; }
}