namespace AspNetCoreMongoApi.Consumers.Keycloak.Login.Events
{
    public record class EventClientUserLoginDetails(
        string AuthMethod,
        string ResponseType,
        string RedirectUri,
        string RememberMe,
        string Consent,
        string CodeId,
        string ResponseMode,
        string Username
    );
}
