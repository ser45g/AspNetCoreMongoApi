using AspNetCoreMongoApi.Contracts.Request;
using AspNetCoreMongoApi.Options;
using FastEndpoints;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace AspNetCoreMongoApi.Validators.RequestValidators
{
    public class UpdateWeatherForecastRequestValidator: Validator<UpdateWeatherForecastRequest>
    {
        public UpdateWeatherForecastRequestValidator(IOptions<WeatherForecastValidationOptions> options) {
            RuleFor(w => w.Date).NotNull();
            RuleFor(w => w.TemperatureC).NotNull().InclusiveBetween(options.Value.MinTemperatureC, options.Value.MaxTemperatureC);
            RuleFor(w => w.Summary).Length(options.Value.MinSummaryLength, options.Value.MaxSummaryLength);
        }
    }
}
