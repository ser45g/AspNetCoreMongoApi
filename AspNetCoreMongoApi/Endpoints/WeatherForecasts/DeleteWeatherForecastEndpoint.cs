using AspNetCoreMongoApi.Contracts.Request;
using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.Entities;
using FastEndpoints;

namespace AspNetCoreMongoApi.Endpoints.WeatherForecasts
{
    public class DeleteWeatherForecastEndpoint(MongoDbContext _context):Endpoint<WeatherForecastDeleteRequest>
    {
        public override void Configure()
        {
            Delete("/weather-forecast/{id:guid}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(WeatherForecastDeleteRequest req, CancellationToken ct)
        {
            _context.WeatherForecasts.Remove(new WeatherForecast() { Id = req.Id });

            await _context.SaveChangesAsync();

            await Send.NoContentAsync(ct);
        }
    }
}
