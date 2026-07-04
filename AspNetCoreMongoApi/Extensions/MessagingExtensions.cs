using AspNetCoreMongoApi.Consumers.Keycloak;
using AspNetCoreMongoApi.Options;
using MassTransit;

namespace AspNetCoreMongoApi.Extensions
{
    public static class MessagingExtensions
    {
        public static IServiceCollection AddAppMessaging(this IServiceCollection services, RabbitMqOptions rabbitMqOptions, string keycloakRealmName)
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

                    //I used this listener: https://github.com/aznamier/keycloak-event-listener-rabbitmq

                    cfg.ReceiveEndpoint("keycloak-events:user-deleted-queue", e =>
                    {
                        e.Durable = true;
                        e.UseRawJsonDeserializer(isDefault: true);
                        e.Bind("keycloak-events", b =>
                        {
                            //b.RoutingKey = "KK.EVENT.*.auth_demo.#"; // this is to handle all keycloak realm events
                            b.RoutingKey = $"KK.EVENT.ADMIN.{keycloakRealmName}.SUCCESS.USER.DELETE";
                            b.ExchangeType = "topic";
                        });
                        e.ConfigureConsumeTopology = false;
                        e.ConfigureConsumer<UserDeletedConsumer>(context);
                    });

                    cfg.ReceiveEndpoint("keycloak-events:user-created-queue", e =>
                    {
                        e.Durable = true;
                        e.UseRawJsonDeserializer(isDefault: true);
                        e.Bind("keycloak-events", b =>
                        {
                            //b.RoutingKey = "KK.EVENT.*.auth_demo.#"; // this is to handle all keycloak realm events
                            b.RoutingKey = $"KK.EVENT.ADMIN.{keycloakRealmName}.SUCCESS.USER.CREATE";
                            b.ExchangeType = "topic";
                        });
                        e.ConfigureConsumeTopology = false;
                        e.ConfigureConsumer<UserCreatedConsumer>(context);
                    });

                    cfg.ConfigureEndpoints(context);
                });

            });
            return services;
        }
    }
}
