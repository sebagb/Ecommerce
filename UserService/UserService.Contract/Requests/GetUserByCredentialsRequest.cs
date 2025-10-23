namespace UserService.Contract.Requests;

public class GetUserByCredentialsRequest
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}
