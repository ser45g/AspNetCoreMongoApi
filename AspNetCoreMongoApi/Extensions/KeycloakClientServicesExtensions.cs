using AspNetCoreMongoApi.Options;
using Duende.AccessTokenManagement;
using Keycloak.AuthServices.Sdk;

namespace AspNetCoreMongoApi.Extensions
{
    public static class KeycloakClientServicesExtensions
    {
        public static IServiceCollection AddKeycloakClientServices(this IServiceCollection services,KeycloakClientOptions keycloakClientOptions, string tokenClientName = "token-client") {

            services.AddClientCredentialsTokenManagement()
                .AddClient(tokenClientName, client =>
                {
                    client.ClientId = ClientId.Parse(keycloakClientOptions.ClientId);
                    client.ClientSecret = ClientSecret.Parse(keycloakClientOptions.Secret);
                    client.TokenEndpoint = new Uri(keycloakClientOptions.TokenUrl);
                }
                );
            services.AddKeycloakAdminHttpClient(o =>
            {
                o.Realm = keycloakClientOptions.Realm;
                o.SslRequired = keycloakClientOptions.RequireHttps ? "all" : "none";
                o.AuthServerUrl = keycloakClientOptions.AuthServerUrl;
                o.Credentials = new Keycloak.AuthServices.Common.KeycloakClientInstallationCredentials() { Secret = keycloakClientOptions.Secret };
                o.VerifyTokenAudience = true;
                o.Resource = keycloakClientOptions.ClientId;
                o.TokenClockSkew = TimeSpan.Zero;
            }).AddClientCredentialsTokenHandler(ClientCredentialsClientName.Parse(tokenClientName));

            return services;
        }
    }
}
