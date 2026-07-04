using AspNetCoreMongoApi.Options;
using AspNetCoreMongoApi.Validators.OptionsValidators;
using FluentValidation;

namespace AspNetCoreMongoApi.Extensions
{
    public static class ValidationExtensions
    {
        public static IServiceCollection AddValidatedApplicationOptions(this IServiceCollection services) {

            services.AddOptions<DbOptions>().BindConfiguration(DbOptions.ConfigurationSection).ValidateOnStart().Validate(mongoDbOptions =>
            {
                var validator = new DbOptionsValidator();
                var validationResult = validator.Validate(mongoDbOptions);

                return validationResult.IsValid;
            });

            services.AddOptions<AuthenticationOptions>().BindConfiguration(AuthenticationOptions.ConfigurationSection).ValidateOnStart().Validate(authOptions =>
            {
                var validator = new AuthenticationOptionsValidator();
                var validationResult = validator.Validate(authOptions);

                return validationResult.IsValid;
            });

            services.AddOptions<KeycloakClientOptions>().BindConfiguration(KeycloakClientOptions.ConfigurationSection).ValidateOnStart().Validate(keycloakClientOptions =>
            {
                var validator = new KeycloakClientOptionsValidator();
                var validationResult = validator.Validate(keycloakClientOptions);

                return validationResult.IsValid;
            });

            services.AddOptions<PaginationOptions>().BindConfiguration(PaginationOptions.ConfigurationSection);

            services.AddOptions<WeatherForecastValidationOptions>().BindConfiguration(WeatherForecastValidationOptions.ConfigurationSection);

            services.AddOptions<RedisOptions>().BindConfiguration(RedisOptions.ConfigurationSection).ValidateOnStart().Validate(redisOptions =>
            {
                var validator = new RedisOptionsValidator();
                var validationResult = validator.Validate(redisOptions);

                return validationResult.IsValid;
            });

            services.AddOptions<
                ElasticSearchOptions>().BindConfiguration(ElasticSearchOptions.ConfigurationSection).ValidateOnStart().Validate(elasticSearchOptions =>
            {
                var validator = new ElasticSearchOptionsValidator();
                var validationResult = validator.Validate(elasticSearchOptions);

                return validationResult.IsValid;
            });

            services.AddOptions<CorsOptions>().BindConfiguration(CorsOptions.ConfigurationSection);

            services.AddOptions<
                RabbitMqOptions>().BindConfiguration(RabbitMqOptions.ConfigurationSection).ValidateOnStart().Validate(rabbitMqOptions =>
                {
                    var validator = new RabbitMqOptionsValidator();
                    var validationResult = validator.Validate(rabbitMqOptions);

                    return validationResult.IsValid;
                });

            return services;
        }
    }
}
