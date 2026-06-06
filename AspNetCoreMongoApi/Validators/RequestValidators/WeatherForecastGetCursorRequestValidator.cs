using AspNetCoreMongoApi.Contracts.Request;
using AspNetCoreMongoApi.Options;
using FastEndpoints;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace AspNetCoreMongoApi.Validators.RequestValidators
{
    public class WeatherForecastGetCursorRequestValidator:Validator<WeatherForecastGetCursorRequest>
    {
        public WeatherForecastGetCursorRequestValidator(IOptions<WeatherForecastPaginationOptions> options) {

            RuleFor(r => r.PageSize).InclusiveBetween(options.Value.MinPageSize, options.Value.MaxPageSize);
        }
    }
}
