namespace UserService.Api.Auth;

public static class AuthConstants
{
    public const string UserIdClaimName = "userId";
    public const string Issuer = "https://id.ecommerce.com";
    public const string Audience = "https://ecommerce.com";
    public const string TokenSecret =
        "PleasePleaseStoreAndLoadSecurelyTheTokenSecret";
}