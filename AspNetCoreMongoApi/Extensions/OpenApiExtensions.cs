using AspNetCoreMongoApi.Options;
using Microsoft.OpenApi;
using Scalar.AspNetCore;

namespace AspNetCoreMongoApi.Extensions
{
    public static class OpenApiExtensions
    {
        public static IServiceCollection AddOpenApiWithOIDC(this IServiceCollection services, AuthenticationOptions authenticationOptions) {
            services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    document.Components ??= new OpenApiComponents();
                    document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

                    document.Components.SecuritySchemes["OAuth2"] = new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows
                        {
                            AuthorizationCode = new OpenApiOAuthFlow
                            {
                                AuthorizationUrl = new Uri(authenticationOptions.AuthorizationUrl),
                                TokenUrl = new Uri(authenticationOptions.TokenUrl),
                                RefreshUrl = new Uri(authenticationOptions.RefreshUrl),
                                Scopes = authenticationOptions.Scopes
                            }
                        }
                    };

                    return Task.FromResult(document); // ✅ Return the document
                });
            });
            return services;
        }

        public static IEndpointConventionBuilder MapOpenScalarReferenceWithOIDC(this IEndpointRouteBuilder app, AuthenticationOptions authenticationOptions) {

            return app.MapScalarApiReference(options => options.AddPreferredSecuritySchemes("OAuth2").AddAuthorizationCodeFlow("OAuth2", flow =>
            {
                flow.ClientId = authenticationOptions.ClientId;
                flow.AuthorizationUrl = authenticationOptions.AuthorizationUrl;
                flow.TokenUrl = authenticationOptions.TokenUrl;
                flow.Pkce = Pkce.Sha256;
                flow.WithSelectedScopes(authenticationOptions.Scopes?.Keys);
                flow.RefreshUrl = authenticationOptions.RefreshUrl;
                flow.AddBodyParameter("client_id", authenticationOptions.ClientId);
            }));

        }
    }
}
