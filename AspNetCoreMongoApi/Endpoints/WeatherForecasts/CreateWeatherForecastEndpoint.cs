using AspNetCoreMongoApi.Contracts.Request;
using AspNetCoreMongoApi.Contracts.Response;
using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.Entities;
using AutoMapper;
using FastEndpoints;
using FluentValidation;
using IMapper = AutoMapper.IMapper;

namespace AspNetCoreMongoApi.Endpoints.WeatherForecasts
{
    
    public class CreateWeatherForecastEndpoint(IMapper _mapper, IValidator<WeatherForecastCreateRequest> validator, MongoDbContext _context):Endpoint<WeatherForecastCreateRequest, WeatherForecastResponse>
    {
        public override void Configure()
        {
            Post("/weather-forecast");
            AllowAnonymous();
        }

        public override async Task HandleAsync(WeatherForecastCreateRequest req, CancellationToken ct)
        {
            var weatherForecast = _mapper.Map<WeatherForecast>(req);

            _context.WeatherForecasts.Add(weatherForecast);

            await _context.SaveChangesAsync();

            var response = _mapper.Map<WeatherForecastResponse>(weatherForecast);

            await Send.CreatedAtAsync<GetWeatherForecastByIdEndpoint>(new { id = response.Id }, response, cancellation: ct);
        }

        
    }
}
