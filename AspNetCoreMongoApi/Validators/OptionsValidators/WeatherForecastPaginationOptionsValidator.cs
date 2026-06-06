using AspNetCoreMongoApi.Options;
using FluentValidation;

namespace AspNetCoreMongoApi.Validators.OptionsValidators
{

    public class WeatherForecastPaginationOptionsValidator : AbstractValidator<WeatherForecastPaginationOptions>
    { 
        public WeatherForecastPaginationOptionsValidator()
        {
            RuleFor(o => o.DefaultPageSize).NotNull();
            RuleFor(o => o.MaxPageSize).NotNull();

        }

    }
}
