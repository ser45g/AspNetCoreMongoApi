using AspNetCoreMongoApi.Contracts.Request;
using AspNetCoreMongoApi.Contracts.Response;
using AspNetCoreMongoApi.Data;
using AutoMapper;
using FastEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IMapper = AutoMapper.IMapper;

namespace AspNetCoreMongoApi.Endpoints.WeatherForecasts
{
    public class GetWeatherForecastByIdEndpoint(IMapper _mapper, MongoDbContext _context):Endpoint<GetByIdWeatherForecastRequest, WeatherForecastResponse>
    {
        public override void Configure()
        {
            Get("/weather-forecast/{id:guid}");
        }

        public override async Task HandleAsync( GetByIdWeatherForecastRequest req, CancellationToken ct)
        {
            var weatherForecast = await _context.WeatherForecasts.FirstOrDefaultAsync(w => w.Id == req.Id);


            if (weatherForecast == null)
            {
                await Send.NotFoundAsync(cancellation: ct);
            }
            var response = _mapper.Map<WeatherForecastResponse>(weatherForecast);
            await Send.OkAsync(response, ct);
        }
    }
}
