using AspNetCoreMongoApi.Contracts.Todos.Request;
using AspNetCoreMongoApi.Contracts.Todos.Response;
using AspNetCoreMongoApi.Contracts.WeatherForecasts.Request;
using AspNetCoreMongoApi.Contracts.WeatherForecasts.Response;
using AspNetCoreMongoApi.Entities;

namespace AspNetCoreMongoApi.Extensions.Mappers
{
    public static class WeatherForecastMapperExtensions
    {
        public static WeatherForecastResponse ToWeatherForecastResponse(this WeatherForecast weatherForecast)
        {
            return new WeatherForecastResponse(weatherForecast.Id, weatherForecast.Date, weatherForecast.TemperatureC, weatherForecast.TemperatureF, weatherForecast.Summary);
        }

        public static WeatherForecast ToWeatherForecast(this CreateWeatherForecastRequest req)
        {
            return new WeatherForecast() { Date= req.Date!.Value, Summary = req.Summary, TemperatureC=req.TemperatureC!.Value};
        }
    }
}
