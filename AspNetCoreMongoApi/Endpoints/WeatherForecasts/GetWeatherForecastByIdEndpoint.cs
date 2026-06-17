using AspNetCoreMongoApi.Contracts.WeatherForecasts.Request;
using AspNetCoreMongoApi.Contracts.WeatherForecasts.Response;
using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.Extensions.Mappers;
using AspNetCoreMongoApi.Helpers;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using ZiggyCreatures.Caching.Fusion;

namespace AspNetCoreMongoApi.Endpoints.WeatherForecasts
{
    public class GetWeatherForecastByIdEndpoint(AppDbContext context, IFusionCache hybridCache):Endpoint<GetByIdWeatherForecastRequest, WeatherForecastResponse>
    {
        public override void Configure()
        {
            Get(EndpointRoutes.WeatherForecast+ "/{id:guid}");
        }

        public override async Task HandleAsync(GetByIdWeatherForecastRequest req, CancellationToken ct)
        {
            var weatherForecast = await hybridCache.GetOrSetAsync(CacheKeys.WeatherForecastById(req.Id), async cancellationToken => { 
                return await context.WeatherForecasts.FirstOrDefaultAsync(w => w.Id == req.Id, cancellationToken: cancellationToken);
            }, token:ct); 

            if (weatherForecast == null)
            {
                await Send.NotFoundAsync(cancellation: ct);
                return;
            }
            var response = weatherForecast.ToWeatherForecastResponse();
            await Send.OkAsync(response, ct);
        }
    }
}
