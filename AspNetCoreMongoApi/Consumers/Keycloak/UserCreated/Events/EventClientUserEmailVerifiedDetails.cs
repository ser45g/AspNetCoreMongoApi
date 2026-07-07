namespace AspNetCoreMongoApi.Consumers.Keycloak.UserCreated.Events
{
    public record class EventClientUserEmailVerifiedDetails(
        string AuthMethod,
        string TokenId,
        string Action,
        string ResponseType,
        string RedirectUri,
        string RememberMe,
        string CodeId,
        string Email,
        string ResponseMode,
        string Username
    );
}
