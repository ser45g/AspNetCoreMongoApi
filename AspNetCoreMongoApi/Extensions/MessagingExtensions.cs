using AspNetCoreMongoApi.Consumers.Cleanup;
using AspNetCoreMongoApi.Consumers.Keycloak;
using AspNetCoreMongoApi.Consumers.Keycloak.UserCreated;
using AspNetCoreMongoApi.Consumers.Keycloak.UserDeleted;
using AspNetCoreMongoApi.Helpers;
using AspNetCoreMongoApi.Options;
using MassTransit;

namespace AspNetCoreMongoApi.Extensions
{
    public static class MessagingExtensions
    {
        public static IServiceCollection AddAppMessaging(this IServiceCollection services, RabbitMqOptions rabbitMqOptions, string keycloakRealmName, string keycloakClientId, bool logRawMessages=false)
        {
            services.AddMassTransit(config =>
            {
                config.SetKebabCaseEndpointNameFormatter();
                config.AddConsumers(typeof(Program).Assembly);

                config.UsingRabbitMq((context, cfg) =>
                {
                    cfg.UseMessageRetry(r => {
                        r.Incremental(5, TimeSpan.FromMilliseconds(200), TimeSpan.FromMilliseconds(250));
                    });

                    cfg.Host(rabbitMqOptions.Url, o =>
                    {
                        o.Username(rabbitMqOptions.Username);
                        o.Password(rabbitMqOptions.Password);
                    });

                    cfg.ReceiveEndpoint("keycloak-events.admin.user-deleted-queue", e =>
                    {
                        e.ConfigureConsumeTopology = false;

                        e.Durable = true;
                        e.UseRawJsonDeserializer(isDefault: true);

                        e.Bind("keycloak-events", b =>
                        {
                            b.RoutingKey = KeycloakEvents.AccountDeletedAdmin(keycloakRealmName);
                            b.ExchangeType = "topic";
                        });
                        e.ConfigureConsumer<UserDeleted_SendEmailNotification_Admin_Consumer>(context);
                        e.ConfigureConsumer<UserDeleted_Cleanup_Admin_Consumer>(context);
                    });

                    cfg.ReceiveEndpoint("keycloak-events.client.user-deleted-queue", e =>
                    {
                        e.ConfigureConsumeTopology = false;

                        e.Durable = true;
                        e.UseRawJsonDeserializer(isDefault: true);

                        e.Bind("keycloak-events", b =>
                        {
                            b.RoutingKey = KeycloakEvents.AccountDeletedClient(keycloakRealmName);
                            b.ExchangeType = "topic";
                        });

                        e.ConfigureConsumer<UserDeleted_SendEmailNotification_Client_Consumer>(context);
                        e.ConfigureConsumer<UserDeleted_Cleanup_Client_Consumer>(context);

                    });

                    cfg.ReceiveEndpoint("keycloak-events.admin.user-created-queue", e =>
                    {
                        e.ConfigureConsumeTopology = false;

                        e.Durable = true;
                        e.UseRawJsonDeserializer(isDefault: true);

                        e.Bind("keycloak-events", b =>
                        {
                            b.RoutingKey = KeycloakEvents.AccountCreatedAdmin(keycloakRealmName);
                            b.ExchangeType = "topic";
                        });
                        e.ConfigureConsumer<UserCreated_Admin_Consumer>(context);
                    });

                    cfg.ReceiveEndpoint("keycloak-events.client.user-created-queue", e =>
                    {
                        e.ConfigureConsumeTopology = false;

                        e.Durable = true;
                        e.UseRawJsonDeserializer(isDefault: true);

                        e.Bind("keycloak-events", b =>
                        {
                            b.RoutingKey = KeycloakEvents.EmailVerifiedClient(keycloakRealmName, keycloakClientId);
                            b.ExchangeType = "topic";
                        });
                        e.ConfigureConsumer<UserCreated_Client_Consumer>(context);
                    });

                    cfg.Message<CleanupByUserIdEvent>(m =>
                    {
                        m.SetEntityName(AppEvents.CleanupByUserIdExchange);
                    });

                    cfg.Publish<CleanupByUserIdEvent>(p =>
                    {
                        p.ExchangeType = "topic";
                    });

                    cfg.ReceiveEndpoint(AppEvents.CleanupByUserIdQueue, e =>
                    {
                        e.ConfigureConsumeTopology = false;

                        e.Durable = true;
                        e.UseRawJsonDeserializer(isDefault: true);

                        e.Bind(AppEvents.CleanupByUserIdExchange, b =>
                        {
                            b.RoutingKey = AppEvents.CleanupByUserIdRoutingKey;
                            b.ExchangeType = "topic";
                        });

                        e.ConfigureConsumer<CleanupByUserIdConsumer>(context);
                    });

                    if (logRawMessages)
                    {
                        cfg.ReceiveEndpoint("keycloak-events:all-catch-query", e =>
                        {
                            e.ConfigureConsumeTopology = false;

                            e.Durable = true;
                            e.UseRawJsonDeserializer(isDefault: true);

                            e.Bind("keycloak-events", b =>
                            {
                                b.RoutingKey = KeycloakEvents.AllRealmEvents(keycloakRealmName);
                                b.ExchangeType = "topic";
                            });

                            e.ConfigureConsumer<AllCatchConsumer>(context);
                        });
                    }
                });
            });
            return services;
        }
    }
}
