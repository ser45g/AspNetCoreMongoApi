using AspNetCoreMongoApi.Contracts.Request;
using AspNetCoreMongoApi.Contracts.Response;
using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.Entities;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreMongoApi.Endpoints.WeatherForecasts
{
    public class UpdateWeatherForecastEndpoint(AutoMapper.IMapper _mapper, MongoDbContext _context) : Endpoint<UpdateWeatherForecastRequest, WeatherForecastResponse>
    {
        public override void Configure()
        {
            Put("/weather-forecast");
            AllowAnonymous();
        }

        public override async Task HandleAsync(UpdateWeatherForecastRequest req, CancellationToken ct)
        {
            var weatherForecast = _mapper.Map<WeatherForecast>(req);

            await _context.WeatherForecasts.Where(w => w.Id == req.Id).ExecuteUpdateAsync(w => w.SetProperty(w => w.Date, weatherForecast.Date).SetProperty(w => w.TemperatureC, weatherForecast.TemperatureC).SetProperty(w => w.TemperatureF, weatherForecast.TemperatureF).SetProperty(w => w.Summary, weatherForecast.Summary));

            await Send.NoContentAsync(ct);
        }
    }
}
