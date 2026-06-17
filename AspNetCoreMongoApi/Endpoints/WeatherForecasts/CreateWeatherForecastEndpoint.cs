using AspNetCoreMongoApi.Contracts.WeatherForecasts.Request;
using AspNetCoreMongoApi.Contracts.WeatherForecasts.Response;
using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.Extensions.Mappers;
using AspNetCoreMongoApi.Helpers;
using FastEndpoints;

namespace AspNetCoreMongoApi.Endpoints.WeatherForecasts
{
    
    public class CreateWeatherForecastEndpoint(AppDbContext context):Endpoint<CreateWeatherForecastRequest, WeatherForecastResponse>
    {
        public override void Configure()
        {
            Post(EndpointRoutes.WeatherForecast);
            Policies("Admin");
        }

        public override async Task HandleAsync(CreateWeatherForecastRequest req, CancellationToken ct)
        {
            var weatherForecast = req.ToWeatherForecast();

            context.WeatherForecasts.Add(weatherForecast);

            await context.SaveChangesAsync();

            var response = weatherForecast.ToWeatherForecastResponse();

            await Send.CreatedAtAsync<GetWeatherForecastByIdEndpoint>(new { id = response.Id }, response, cancellation: ct);
        }
    }
}
