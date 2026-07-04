namespace AspNetCoreMongoApi.Consumers.Keycloak.Events
{
    public record class UserRepresentation(
        string FirstName,
        string LastName,
        string Email,
        bool EmailVerified,
        bool Enabled,
        IDictionary<string, object> Attributes,
        IEnumerable<string> RequiredActions,
        IEnumerable<string> Groups);
}

