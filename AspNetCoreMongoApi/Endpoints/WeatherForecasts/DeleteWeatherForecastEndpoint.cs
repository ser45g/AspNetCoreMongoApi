using AspNetCoreMongoApi.Contracts.Request;
using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.Entities;
using FastEndpoints;

namespace AspNetCoreMongoApi.Endpoints.WeatherForecasts
{
    public class DeleteWeatherForecastEndpoint(MongoDbContext _context):Endpoint<DeleteWeatherForecastRequest>
    {
        public override void Configure()
        {
            Delete("/weather-forecast/{id:guid}");
            AllowAnonymous();
            Description(x => x.ClearDefaultAccepts());
        }

        public override async Task HandleAsync(DeleteWeatherForecastRequest req, CancellationToken ct)
        {
            _context.WeatherForecasts.Remove(new WeatherForecast() { Id = req.Id });

            await _context.SaveChangesAsync();

            await Send.NoContentAsync(ct);
        }
    }
}
