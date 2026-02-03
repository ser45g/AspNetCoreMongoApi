using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.Entities;
using FastEndpoints;

namespace AspNetCoreMongoApi.Endpoints.WeatherForecasts
{
    public class DeleteWeatherForecastEndpoint(MongoDbContext _context):Endpoint<Guid>
    {
        public override void Configure()
        {
            Delete("/weather-forecast/{id:guid}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Guid id, CancellationToken ct)
        {
            _context.WeatherForecasts.Remove(new WeatherForecast() { Id = id });

            await _context.SaveChangesAsync();

            await Send.NoContentAsync(ct);
        }
    }
}
