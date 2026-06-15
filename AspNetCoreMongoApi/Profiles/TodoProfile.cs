using AspNetCoreMongoApi.Contracts.Todos.Request;
using AspNetCoreMongoApi.Contracts.Todos.Response;
using AspNetCoreMongoApi.Entities;
using AutoMapper;

namespace AspNetCoreMongoApi.Profiles
{
    public class TodoProfile : Profile
    {
        public TodoProfile()
        {
            CreateMap<Todo, TodoResponse>();

            CreateMap<CreateTodoRequest, Todo>();

            CreateMap<UpdateTodoRequest, Todo>();
        }
    }
}
