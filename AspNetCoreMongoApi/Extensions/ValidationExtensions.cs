using AspNetCoreMongoApi.Options;
using AspNetCoreMongoApi.Validators.OptionsValidators;

namespace AspNetCoreMongoApi.Extensions
{
    public static class ValidationExtensions
    {
        public static IServiceCollection AddValidatedApplicationOptions(this IServiceCollection services) {

            services.AddOptions<MongoDbOptions>().BindConfiguration(MongoDbOptions.ConfigurationSection).ValidateOnStart().Validate(mongoDbOptions =>
            {
                var validator = new MongoDbOptionsValidator();
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

            services.AddOptions<WeatherForecastPaginationOptions>().BindConfiguration(WeatherForecastPaginationOptions.ConfigurationSection);

            services.AddOptions<WeatherForecastValidationOptions>().BindConfiguration(WeatherForecastValidationOptions.ConfigurationSection);

            services.AddOptions<RedisOptions>().BindConfiguration(RedisOptions.ConfigurationSection).ValidateOnStart().Validate(redisOptions =>
            {
                var validator = new RedisOptionsValidator();
                var validationResult = validator.Validate(redisOptions);

                return validationResult.IsValid;
            });

            services.AddOptions<CorsOptions>().BindConfiguration(CorsOptions.ConfigurationSection);

            return services;
        }
    }
}
