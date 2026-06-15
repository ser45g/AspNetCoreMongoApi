using AspNetCoreMongoApi.Contracts.WeatherForecasts.Request;
using AspNetCoreMongoApi.Options;
using FastEndpoints;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace AspNetCoreMongoApi.Validators.RequestValidators.WeatherForecasts
{
    public class GetCursorWeatherForecastsRequestValidator:Validator<GetCursorWeatherForecastsRequest>
    {
        public GetCursorWeatherForecastsRequestValidator(IOptions<PaginationOptions> options) {

            RuleFor(r => r.PageSize).InclusiveBetween(options.Value.MinPageSize, options.Value.MaxPageSize);
        }
    }
}
