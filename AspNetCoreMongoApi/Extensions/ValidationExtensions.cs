using AspNetCoreMongoApi.Options;
using AspNetCoreMongoApi.Validators.OptionsValidators;

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

            services.AddOptions<SeqOptions>().BindConfiguration(SeqOptions.ConfigurationSection).ValidateOnStart().Validate(seqOptions =>
            {
                var validator = new SeqOptionsValidator();
                var validationResult = validator.Validate(seqOptions);

                return validationResult.IsValid;
            });

            services.AddOptions<AuthenticationOptions>().BindConfiguration(AuthenticationOptions.ConfigurationSection).ValidateOnStart().Validate(authOptions =>
            {
                var validator = new AuthenticationOptionsValidator();
                var validationResult = validator.Validate(authOptions);

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

            return services;
        }
    }
}
