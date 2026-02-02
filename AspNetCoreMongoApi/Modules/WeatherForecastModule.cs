using AspNetCoreMongoApi.Contracts;
using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.Entities;
using AutoMapper;
using Carter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreMongoApi.Modules
{
    public class WeatherForecastModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {

            var group = app.MapGroup("/weather-forecast");


            group.MapGet("/", async ([FromServices] IMapper mapper, [FromServices] MongoDbContext context, [FromServices] ILogger<WeatherForecastModule> logger) =>
            {
                var weatherForecasts = await context.WeatherForecasts.ToListAsync();

                var response = mapper.Map<IEnumerable<WeatherForecastDto>>(weatherForecasts);

            
                return Results.Ok(response);
            }).WithName("GetWeatherForecasts");
         

            group.MapGet("{id:guid}", async (Guid id, [FromServices] IMapper mapper, [FromServices] MongoDbContext context, [FromServices] ILogger<WeatherForecastModule> logger) =>
            {
                var weatherForecast = await context.WeatherForecasts.FirstOrDefaultAsync(w => w.Id == id);
              

                if (weatherForecast == null) {
                    return Results.NotFound<Guid>(id);
                }
                var response = mapper.Map<WeatherForecastDto>(weatherForecast);
                return Results.Ok<WeatherForecastDto>(response);
            }).WithName("GetWeatherForecastById");


            group.MapPost("/", async (WeatherForecastCreateDto dto, [FromServices] IMapper mapper, [FromServices] MongoDbContext context) =>
            {
                var weatherForecast = mapper.Map<WeatherForecast>(dto);

                context.WeatherForecasts.Add(weatherForecast);

                await context.SaveChangesAsync();
                
                var response = mapper.Map<WeatherForecastDto>(weatherForecast);

                return Results.CreatedAtRoute("GetWeatherForecastById",  new { id = response.Id }, response);
            }).WithName("CreateWeatherForecast");


            group.MapPut("/", async ( WeatherForecastUpdateDto dto, [FromServices] IMapper mapper, [FromServices] MongoDbContext context) =>
            {
                var weatherForecast = mapper.Map<WeatherForecast>(dto);

                await context.WeatherForecasts.Where(w=>w.Id==dto.Id).ExecuteUpdateAsync(w=>w.SetProperty(w=>w.Date,weatherForecast.Date).SetProperty(w=>w.TemperatureC, weatherForecast.TemperatureC).SetProperty(w => w.TemperatureF, weatherForecast.TemperatureF).SetProperty(w=>w.Summary, weatherForecast.Summary));

                return Results.NoContent();

            }).WithName("UpdateWeatherForecastById");


            group.MapDelete("{id:guid}", async (Guid id, [FromServices] MongoDbContext context) =>
            {
                context.WeatherForecasts.Remove(new WeatherForecast() { Id = id });

                await context.SaveChangesAsync();

                return Results.NoContent();
            }).WithName("DeleteWeatherForecastById");

           
        }
    }
}
