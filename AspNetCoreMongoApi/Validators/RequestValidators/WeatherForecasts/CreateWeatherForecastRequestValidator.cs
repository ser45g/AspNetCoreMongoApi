using AspNetCoreMongoApi.Contracts.WeatherForecasts.Request;
using AspNetCoreMongoApi.Options;
using FastEndpoints;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace AspNetCoreMongoApi.Validators.RequestValidators.WeatherForecasts
{
    public class CreateWeatherForecastRequestValidator: Validator<CreateWeatherForecastRequest>
    {
        public CreateWeatherForecastRequestValidator(IOptions<WeatherForecastValidationOptions> options) {
            RuleFor(w => w.Date).NotNull();
            RuleFor(w => w.TemperatureC).NotNull().InclusiveBetween(options.Value.MinTemperatureC, options.Value.MaxTemperatureC);
            RuleFor(w => w.Summary).Length(options.Value.MinSummaryLength, options.Value.MaxSummaryLength);
        }
    }
}
