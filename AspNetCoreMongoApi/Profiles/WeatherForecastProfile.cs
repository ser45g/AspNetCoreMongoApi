using AspNetCoreMongoApi.Contracts.WeatherForecasts.Request;
using AspNetCoreMongoApi.Contracts.WeatherForecasts.Response;
using AspNetCoreMongoApi.Entities;
using AutoMapper;

namespace AspNetCoreMongoApi.Profiles
{
    public class WeatherForecastProfile : Profile
    {
        public WeatherForecastProfile()
        {
            CreateMap<WeatherForecast, WeatherForecastResponse>();

            CreateMap<CreateWeatherForecastRequest, WeatherForecast>();

            CreateMap<UpdateWeatherForecastRequest, WeatherForecast>();
        }
    }
}
