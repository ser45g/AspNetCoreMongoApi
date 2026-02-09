using AspNetCoreMongoApi.Contracts.Response;
using AspNetCoreMongoApi.Data;
using AutoMapper;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreMongoApi.Endpoints.WeatherForecasts
{
    public class GetWeatherForecastsEndpoint(AutoMapper.IMapper _mapper, MongoDbContext _context) : EndpointWithoutRequest<IEnumerable<WeatherForecastDto>>
    {
        public override void Configure()
        {
            Get("/weather-forecast");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var weatherForecasts = await _context.WeatherForecasts.ToListAsync();

            var response = _mapper.Map<IEnumerable<WeatherForecastDto>>(weatherForecasts);

            await Send.OkAsync(response, ct);
        }
       
    }
}
