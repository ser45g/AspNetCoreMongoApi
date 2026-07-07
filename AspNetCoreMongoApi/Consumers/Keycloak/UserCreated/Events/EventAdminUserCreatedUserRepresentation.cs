namespace AspNetCoreMongoApi.Consumers.Keycloak.UserCreated.Events
{
    public record class EventAdminUserCreatedUserRepresentation(
        string FirstName,
        string LastName,
        string Email,
        bool EmailVerified,
        Dictionary<string, object> Attributes,
        bool Enabled,
        List<string> RequiredActions,
        List<string> Groups
    );
}
