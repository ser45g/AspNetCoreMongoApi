using AspNetCoreMongoApi.Options;
using FluentValidation;

namespace AspNetCoreMongoApi.Validators.OptionsValidators
{

    public class WeatherForecastValidationOptionsValidator : AbstractValidator<WeatherForecastValidationOptions>
    { 
        public WeatherForecastValidationOptionsValidator()
        {
            RuleFor(x => x.MinTemperatureC).NotNull();
            RuleFor(x => x.MaxTemperatureC).NotNull();
            RuleFor(x => x.MinSummaryLength).NotNull();
            RuleFor(x => x.MaxSummaryLength).NotNull();
        }
    }
}
