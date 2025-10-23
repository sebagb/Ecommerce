using System.Text.Json;
using ProductService.Application.Repositories;
using ProductService.Contract.Requests;

namespace ProductService.Api;

public static class ApiEndpoints
{
    private const string Base = "products";
    private readonly static string Create = $"{Base}";
    private readonly static string GetById = $"{Base}/{{id}}";
    private readonly static string Update = $"{Base}/{{id}}";

    public static void RegisterProductEndpoints
        (this IEndpointRouteBuilder builder)
    {
        builder.MapPost(Create, CreateProduct);
        builder.MapGet(GetById, GetProductById);
        builder.MapPut(Update, UpdateProduct);
    }

    private static async Task<IResult> CreateProduct(
        HttpRequest request, IProductRepository repo)
    {
        CreateProductRequest? createProductRequest;
        try
        {
            createProductRequest =
                await request.ReadFromJsonAsync<CreateProductRequest>();
        }
        catch (Exception ex)
        {
            if (ex is JsonException || ex is InvalidOperationException)
            {
                return Results.BadRequest("Failed to deserialize request body");
            }
            throw;
        }

        var product = createProductRequest!.MapToProduct();

        repo.Create(product);

        var productResponse = product.MapToResponse();
        return Results.Ok(productResponse);
    }

    private static IResult GetProductById(Guid id, IProductRepository repo)
    {
        var product = repo.GetById(id);
        var response = product.MapToResponse();
        return Results.Ok(response);
    }

    private static async Task<IResult> UpdateProduct
        (HttpRequest request,
        IProductRepository repo)
    {
        UpdateProductRequest? updateProductRequest;
        try
        {
            updateProductRequest =
                await request.ReadFromJsonAsync<UpdateProductRequest>();
        }
        catch (Exception ex)
        {
            if (ex is JsonException || ex is InvalidOperationException)
            {
                return Results.BadRequest("Failed to deserialize request body");
            }
            throw;
        }

        var id = new Guid((string)request.RouteValues["id"]!);
        var product = updateProductRequest!.MapToProduct(id);

        repo.Update(product);

        var response = product.MapToResponse();
        return Results.Ok(response);
    }
}