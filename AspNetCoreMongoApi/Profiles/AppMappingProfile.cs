using AspNetCoreMongoApi.Contracts;
using AspNetCoreMongoApi.Entities;
using AutoMapper;

namespace AspNetCoreMongoApi.Profiles
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<WeatherForecast, WeatherForecastDto>();

            CreateMap<WeatherForecastCreateDto, WeatherForecast>();

            CreateMap<WeatherForecastUpdateDto, WeatherForecast>();
        }
    }
}
