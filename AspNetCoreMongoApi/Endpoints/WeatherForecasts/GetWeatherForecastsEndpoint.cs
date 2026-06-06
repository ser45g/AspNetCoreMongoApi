using AspNetCoreMongoApi.Contracts.Request;
using AspNetCoreMongoApi.Contracts.Response;
using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.Options;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AspNetCoreMongoApi.Endpoints.WeatherForecasts
{
    public class GetWeatherForecastsEndpoint(AutoMapper.IMapper mapper, MongoDbContext dbContext, IOptions<WeatherForecastPaginationOptions> paginationOptions) : Endpoint<WeatherForecastGetCursorRequest, WeatherForecastCursorResponse<IEnumerable<WeatherForecastResponse>>>
    {
        public override void Configure()
        {
            Get("/weather-forecast");
            AllowAnonymous();
        }

        public override async Task HandleAsync(WeatherForecastGetCursorRequest request, CancellationToken ct)
        {
            var startWith = request.Cursor ?? Guid.Empty;
            var pageSize = request.PageSize ?? paginationOptions.Value.DefaultPageSize;

            var weatherForecasts = await dbContext.WeatherForecasts.Where(w => w.Id >= startWith).Take(pageSize+1).OrderBy(w=>w.Id).ToListAsync(ct);

            Guid? cursor = null;

            if(weatherForecasts.Count == request.PageSize + 1)
            {
                var last = weatherForecasts.LastOrDefault();

                if (last != null)
                {
                    cursor = last.Id;
                    weatherForecasts.Remove(last);
                }
            }

            var weatherForecastDtos = mapper.Map<IEnumerable<WeatherForecastResponse>>(weatherForecasts);

            var response = new WeatherForecastCursorResponse<IEnumerable<WeatherForecastResponse>>(cursor, weatherForecastDtos, weatherForecasts.Count);

            await Send.OkAsync(response, ct);
        }
       
    }
}
