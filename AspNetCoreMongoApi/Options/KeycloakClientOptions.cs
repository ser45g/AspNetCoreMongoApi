using Microsoft.AspNetCore.SignalR;
using System.Net;

namespace AspNetCoreMongoApi.Options
{
    public class KeycloakClientOptions
    {
        public static readonly string ConfigurationSection = nameof(KeycloakClientOptions);
        public required string AuthServerUrl {  get; init; }
        public required string ClientId {  get; init; }
        public required string Realm {  get; init; }
        public bool RequireHttps { get; init; } = true;
        public required string Secret { get; init; }
    }
}
