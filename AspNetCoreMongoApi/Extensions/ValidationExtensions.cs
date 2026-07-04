using AspNetCoreMongoApi.Helpers;
using AspNetCoreMongoApi.Options;
using AspNetCoreMongoApi.Validators.OptionsValidators;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace AspNetCoreMongoApi.Extensions
{
    public static class ValidationExtensions
    {
        public static IServiceCollection AddValidatedApplicationOptions(this IServiceCollection services) {


            services.AddOptionsWithFluentValidation<DbOptions>(DbOptions.ConfigurationSection);

            services.AddOptionsWithFluentValidation<AuthenticationOptions>(AuthenticationOptions.ConfigurationSection);

            services.AddOptionsWithFluentValidation<KeycloakClientOptions>(KeycloakClientOptions.ConfigurationSection);

            services.AddOptionsWithFluentValidation<RedisOptions>(RedisOptions.ConfigurationSection);

            services.AddOptionsWithFluentValidation<ElasticSearchOptions>(ElasticSearchOptions.ConfigurationSection);

            services.AddOptionsWithFluentValidation<RabbitMqOptions>(RabbitMqOptions.ConfigurationSection);

            services.AddOptionsWithFluentValidation<MailOptions>(MailOptions.ConfigurationSection);

            services.AddOptions<WeatherForecastValidationOptions>().BindConfiguration(WeatherForecastValidationOptions.ConfigurationSection);

            services.AddOptions<CorsOptions>().BindConfiguration(CorsOptions.ConfigurationSection);

            services.AddOptions<PaginationOptions>().BindConfiguration(PaginationOptions.ConfigurationSection);

            return services;
        }

        public static OptionsBuilder<TOptions> ValidateFluentValidation<TOptions>(this OptionsBuilder<TOptions> builder) where TOptions : class
        {
            builder.Services.AddSingleton<IValidateOptions<TOptions>>(serviceProvider => new FluentValidateOptions<TOptions>(serviceProvider, builder.Name));

            return builder;
        }

        public static IServiceCollection AddOptionsWithFluentValidation<TOptions>(this IServiceCollection services, string configurationSection) where TOptions : class
        {
            services.AddOptions<TOptions>()
                .BindConfiguration(configurationSection)
                .ValidateFluentValidation() // Configure FluentValidation validation
                .ValidateOnStart(); // Validate options on application start

            return services;
        }
    }
}
