namespace AspNetCoreMongoApi.Consumers.Keycloak.SendVerifyEmail.Events
{
    public record class EventClientSendVerifyEmailDetails(
        string AuthMethod,
        string ResponseType,
        string RedirectUri,
        string RememberMe,
        string CodeId,
        string Email,
        string ResponseMode,
        string Username
    );
}
