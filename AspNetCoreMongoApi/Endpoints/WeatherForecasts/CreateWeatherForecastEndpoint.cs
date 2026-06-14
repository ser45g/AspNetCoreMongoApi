using AspNetCoreMongoApi.Contracts.Request;
using AspNetCoreMongoApi.Contracts.Response;
using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.Entities;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace AspNetCoreMongoApi.Endpoints.WeatherForecasts
{
    
    public class CreateWeatherForecastEndpoint(IMapper mapper, MongoDbContext context):Endpoint<CreateWeatherForecastRequest, WeatherForecastResponse>
    {
        public override void Configure()
        {
            Post("/weather-forecast");
        }

        public override async Task HandleAsync(CreateWeatherForecastRequest req, CancellationToken ct)
        {
            var weatherForecast = mapper.Map<WeatherForecast>(req);

            context.WeatherForecasts.Add(weatherForecast);

            await context.SaveChangesAsync();

            var response = mapper.Map<WeatherForecastResponse>(weatherForecast);

            await Send.CreatedAtAsync<GetWeatherForecastByIdEndpoint>(new { id = response.Id }, response, cancellation: ct);
        }
    }
}
