using System.Text.Json;
using UserService.Api.Auth;
using UserService.Application.Repositories;
using UserService.Contract.Requests;
using UserService.Contract.Responses;

namespace UserService.Api;

public static class ApiEndpoints
{
    private const string Base = "users";
    private static string Create { get; } = $"{Base}";
    private static string GetByCredentials { get; } = $"{Base}";
    private static string GetById { get; } = $"{Base}/{{id}}";
    private static string Update { get; } = $"{Base}";

    private static readonly string jsonBodyKey = "jsonBody";

    public static void RegisterUserEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapPost(Create, CreateUser)
            .AddEndpointFilter(BodyValidationFilter<CreateUserRequest>);

        builder.MapGet(GetByCredentials, GetUserByCredentials);

        builder.MapGet(GetById, GetUserById);

        builder.MapPut(Update, UpdateUser)
            .AddEndpointFilter(UpdateUserFilter);
    }

    private static async Task<IResult> CreateUser(
        HttpContext context, IUserRepository repo)
    {
        var request = (CreateUserRequest)context.Items[jsonBodyKey]!;

        var user = request.MapToUser();

        var created = repo.Create(user);
        if (!created)
        {
            return Results.InternalServerError();
        }

        var userResponse = user.MapToResponse();
        return Results.Ok(userResponse);
    }

    private static IResult GetUserByCredentials(
        HttpRequest request,
        IUserRepository repo)
    {
        var credentials = request.Query.MapToGetUserByCredentialsRequest();
        var username = credentials.Username;
        var password = credentials.Password;

        var user = repo.GetByCredentials(username, password);

        if (user == null)
        {
            return Results.NotFound();
        }

        var jwt = UserToken.CreateUserToken(user);

        var response = new JwtResponse { Access_token = jwt };

        return Results.Ok(response);
    }

    private static IResult GetUserById(Guid id, IUserRepository repo)
    {
        var user = repo.GetById(id);
        if (user == null)
        {
            return Results.NoContent();
        }
        var response = user.MapToResponse();
        return Results.Ok(response);
    }

    private static async Task<IResult> UpdateUser(
        HttpContext context,
        IUserRepository repo)
    {
        var userIdClaim = context.User
            .FindFirst(AuthConstants.UserIdClaimName)!
            .Value;

        var updateUserRequest = (UpdateUserRequest)context.Items[jsonBodyKey]!;

        var userId = new Guid(userIdClaim);
        var user = updateUserRequest!.MapToUser(userId);

        var isUpdated = repo.Update(user);
        if (!isUpdated)
        {
            return Results.InternalServerError();
        }

        var response = user.MapToResponse();
        return Results.Ok(response);
    }

    private static async ValueTask<object?> BodyValidationFilter<T>(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        try
        {
            var request = await context.HttpContext.Request.ReadFromJsonAsync<T>();

            context.HttpContext.Items.Add(jsonBodyKey, request);
        }
        catch (Exception ex)
        {
            if (ex is JsonException || ex is InvalidOperationException)
            {
                return Results.BadRequest(ex.Message);
            }
            throw;
        }

        return await next(context);
    }

    private static async ValueTask<object?> UpdateUserFilter(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var userIdClaim = context.HttpContext.User
            .FindFirst(AuthConstants.UserIdClaimName);

        if (userIdClaim == null)
        {
            return Results.BadRequest("Invalid JWT");
        }

        return await BodyValidationFilter<UpdateUserRequest>(context, next);
    }
}