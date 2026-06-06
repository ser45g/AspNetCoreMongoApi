using AspNetCoreMongoApi.Contracts.Request;
using AspNetCoreMongoApi.Contracts.Response;
using AspNetCoreMongoApi.Entities;
using AutoMapper;

namespace AspNetCoreMongoApi.Profiles
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<WeatherForecast, WeatherForecastResponse>();

            CreateMap<WeatherForecastCreateRequest, WeatherForecast>();

            CreateMap<WeatherForecastUpdateRequest, WeatherForecast>();
        }
    }
}
