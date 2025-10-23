using UserService.Application.Models;
using UserService.Contract.Requests;
using UserService.Contract.Responses;

namespace UserService.Api;

public static class ContractMapping
{
    public static User MapToUser(this CreateUserRequest request)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Password = request.Password,
            Email = request.Email,
            CreatedAt = DateOnly.FromDateTime(DateTime.Now)
        };
    }

    public static User MapToUser(this UpdateUserRequest request, Guid id)
    {
        return new User
        {
            Id = id,
            Username = request.Username,
            Password = request.Password,
            Email = request.Email,
            CreatedAt = DateOnly.FromDateTime(DateTime.Now)
        };
    }


    public static UserResponse MapToResponse(this User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            Username = user.Username,
            Password = user.Password,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        };
    }

    public static GetUserByCredentialsRequest MapToGetUserByCredentialsRequest(
        this IQueryCollection query)
    {
        return new GetUserByCredentialsRequest
        {
            Username = query["username"]!,
            Password = query["password"]!
        };
    }
}