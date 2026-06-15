using AspNetCoreMongoApi.Contracts.WeatherForecasts.Request;
using AspNetCoreMongoApi.Contracts.WeatherForecasts.Response;
using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.Entities;
using AspNetCoreMongoApi.Helpers;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace AspNetCoreMongoApi.Endpoints.WeatherForecasts
{
    
    public class CreateWeatherForecastEndpoint(IMapper mapper, MongoDbContext context):Endpoint<CreateWeatherForecastRequest, WeatherForecastResponse>
    {
        public override void Configure()
        {
            Post(EndpointRoutes.WeatherForecast);
            Policies("Admin");
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
