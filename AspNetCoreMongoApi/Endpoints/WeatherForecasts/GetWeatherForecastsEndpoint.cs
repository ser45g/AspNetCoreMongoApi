using AspNetCoreMongoApi.Contracts.Request;
using AspNetCoreMongoApi.Contracts.Response;
using AspNetCoreMongoApi.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreMongoApi.Endpoints.WeatherForecasts
{
    public class GetWeatherForecastsEndpoint(AutoMapper.IMapper _mapper, MongoDbContext _context) : Endpoint<WeatherForecastGetCursorRequest, WeatherForecastCursorResponse<IEnumerable<WeatherForecastDto>>>
    {
        public override void Configure()
        {
            Get("/weather-forecast");
            AllowAnonymous();
        }

        public override async Task HandleAsync(WeatherForecastGetCursorRequest request, CancellationToken ct)
        {
            var startWith = request.Cursor ?? Guid.Empty;

            var weatherForecasts = await _context.WeatherForecasts.Where(w => w.Id >= startWith).Take(request.PageSize+1).OrderBy(w=>w.Id).ToListAsync(ct);

            Guid? cursor = null;

            if(weatherForecasts.Count == request.PageSize + 1)
            {
                var last = weatherForecasts.LastOrDefault();

                cursor = last?.Id;

                if (last != null)
                    weatherForecasts.Remove(last);
            }

            var weatherForecastDtos = _mapper.Map<IEnumerable<WeatherForecastDto>>(weatherForecasts);

            var response = new WeatherForecastCursorResponse<IEnumerable<WeatherForecastDto>>(cursor, weatherForecastDtos, weatherForecasts.Count);

            await Send.OkAsync(response, ct);
        }
       
    }
}
