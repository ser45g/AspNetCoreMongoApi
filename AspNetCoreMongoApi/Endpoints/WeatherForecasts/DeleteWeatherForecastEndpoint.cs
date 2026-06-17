using AspNetCoreMongoApi.Contracts.WeatherForecasts.Request;
using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.Entities;
using AspNetCoreMongoApi.Helpers;
using FastEndpoints;
using Microsoft.Extensions.Caching.Hybrid;
using ZiggyCreatures.Caching.Fusion;

namespace AspNetCoreMongoApi.Endpoints.WeatherForecasts
{
    public class DeleteWeatherForecastEndpoint(AppDbContext context, IFusionCache hybridCache):Endpoint<DeleteWeatherForecastRequest>
    {
        public override void Configure()
        {
            Delete(EndpointRoutes.WeatherForecast+"/{id:guid}");
            Description(x => x.ClearDefaultAccepts());
            Policies("Admin");
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
