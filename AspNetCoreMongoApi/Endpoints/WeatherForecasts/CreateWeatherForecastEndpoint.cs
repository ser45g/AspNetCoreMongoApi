using AspNetCoreMongoApi.Contracts;
using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.Entities;
using AutoMapper;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace AspNetCoreMongoApi.Endpoints.WeatherForecasts
{
    
    public class CreateWeatherForecastEndpoint(IMapper _mapper, MongoDbContext _context):Endpoint<WeatherForecastCreateDto, WeatherForecastDto>
    {
        public override void Configure()
        {
            Post("/weather-forecast");
            
            AllowAnonymous();
        }

        public override async Task HandleAsync(WeatherForecastCreateDto req, CancellationToken ct)
        {
            var weatherForecast = _mapper.Map<WeatherForecast>(req);

            _context.WeatherForecasts.Add(weatherForecast);

            await _context.SaveChangesAsync();

            var response = _mapper.Map<WeatherForecastDto>(weatherForecast);

            await Send.CreatedAtAsync<GetWeatherForecastByIdEndpoint>(new { id = response.Id }, response, cancellation: ct);
        }
    }
}
