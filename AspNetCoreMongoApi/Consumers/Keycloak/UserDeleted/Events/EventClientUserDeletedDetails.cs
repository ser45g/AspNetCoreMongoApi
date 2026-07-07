namespace AspNetCoreMongoApi.Consumers.Keycloak.UserDeleted.Events
{
    public record EventClientUserDeletedDetails(
        string AuthMethod,
        string CustomRequiredAction,
        string ResponseType,
        string RedirectUri,
        string RememberMe,
        string CodeId,
        string ResponseMode,
        string Username
    );
}
