using AspNetCoreMongoApi.Contracts.WeatherForecasts.Request;
using AspNetCoreMongoApi.Contracts.WeatherForecasts.Response;
using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.Helpers;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using ZiggyCreatures.Caching.Fusion;

namespace AspNetCoreMongoApi.Endpoints.WeatherForecasts
{
    public class UpdateWeatherForecastEndpoint(AppDbContext context, IFusionCache hybridCache) : Endpoint<UpdateWeatherForecastRequest, WeatherForecastResponse>
    {
        public override void Configure()
        {
            Put(EndpointRoutes.WeatherForecast);
            Policies("Admin");
        }

        public override async Task HandleAsync(UpdateWeatherForecastRequest req, CancellationToken ct)
        {
            await hybridCache.RemoveAsync(CacheKeys.WeatherForecastById(req.Id), token: ct);

            var weatherForecast = await context.WeatherForecasts.FirstOrDefaultAsync(w => w.Id == req.Id, cancellationToken: ct);

            if (weatherForecast == null)
            {
                await Send.NotFoundAsync(cancellation: ct);
                return;
            }   

            weatherForecast.Date = req.Date!.Value;
            weatherForecast.TemperatureC = req.TemperatureC!.Value;
            weatherForecast.Summary = req.Summary;

            await context.SaveChangesAsync(ct);

            await Send.NoContentAsync(ct);
        }
    }
}
