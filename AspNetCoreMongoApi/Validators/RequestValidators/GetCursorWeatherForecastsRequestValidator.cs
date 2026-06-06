using AspNetCoreMongoApi.Contracts.Request;
using AspNetCoreMongoApi.Options;
using FastEndpoints;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace AspNetCoreMongoApi.Validators.RequestValidators
{
    public class GetCursorWeatherForecastsRequestValidator:Validator<GetCursorWeatherForecastsRequest>
    {
        public GetCursorWeatherForecastsRequestValidator(IOptions<WeatherForecastPaginationOptions> options) {

            RuleFor(r => r.PageSize).InclusiveBetween(options.Value.MinPageSize, options.Value.MaxPageSize);
        }
    }
}
