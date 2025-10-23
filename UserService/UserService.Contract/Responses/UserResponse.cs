namespace UserService.Contract.Responses;

public class UserResponse
{
    public required Guid Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required DateOnly CreatedAt { get; set; }
}