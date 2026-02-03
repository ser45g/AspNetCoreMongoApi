using AspNetCoreMongoApi.Contracts;
using AspNetCoreMongoApi.Data;
using AutoMapper;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using IMapper = AutoMapper.IMapper;

namespace AspNetCoreMongoApi.Endpoints.WeatherForecasts
{
    public class GetWeatherForecastByIdEndpoint(IMapper _mapper, MongoDbContext _context):Endpoint<Guid, WeatherForecastDto>
    {
        public override void Configure()
        {
            Get("weather-forecast/{id:guid}");
            AllowAnonymous();
        }

        public override async Task HandleAsync( Guid req, CancellationToken ct)
        {
            var weatherForecast = await _context.WeatherForecasts.FirstOrDefaultAsync(w => w.Id == req);


            if (weatherForecast == null)
            {
                await Send.NotFoundAsync(cancellation: ct);
            }
            var response = _mapper.Map<WeatherForecastDto>(weatherForecast);
            await Send.OkAsync(response, ct);
        }
    }
}
