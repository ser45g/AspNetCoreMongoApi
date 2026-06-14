using AspNetCoreMongoApi.Contracts.Request;
using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.Entities;
using AspNetCoreMongoApi.Helpers;
using FastEndpoints;
using Microsoft.Extensions.Caching.Hybrid;
using ZiggyCreatures.Caching.Fusion;

namespace AspNetCoreMongoApi.Endpoints.WeatherForecasts
{
    public class DeleteWeatherForecastEndpoint(MongoDbContext context, IFusionCache hybridCache):Endpoint<DeleteWeatherForecastRequest>
    {
        public override void Configure()
        {
            Delete("/weather-forecast/{id:guid}");
            Description(x => x.ClearDefaultAccepts());
        }

        public override async Task HandleAsync(DeleteWeatherForecastRequest req, CancellationToken ct)
        {
            await hybridCache.RemoveAsync(CacheKeys.WeatherForecastById(req.Id), token:ct);

            context.WeatherForecasts.Remove(new WeatherForecast() { Id = req.Id });

            await context.SaveChangesAsync();

            await Send.NoContentAsync(ct);
        }
    }
}
