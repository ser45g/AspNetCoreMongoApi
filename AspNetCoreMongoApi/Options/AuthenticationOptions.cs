namespace AspNetCoreMongoApi.Options
{
    public class AuthenticationOptions
    {
        public static readonly string ConfigurationSection = nameof(AuthenticationOptions);
        public required string MetadataAddress {  get; init; }
        public required string ValidIssuer {  get; init; }
        public required string AuthorizationUrl {  get; init; }
        public required string TokenUrl {  get; init; }
        public required string RefreshUrl {  get; init; }
        public required string Audience {  get; init; }
        public required string ClientId {  get; init; }
        public bool RequireHttps { get; init; } = true;
        public IDictionary<string, string>? Scopes { get; init; }
    }
}
