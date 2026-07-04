using AspNetCoreMongoApi.Contracts.Common.Response;
using AspNetCoreMongoApi.Contracts.Todos.Request;
using AspNetCoreMongoApi.Contracts.Todos.Response;
using AspNetCoreMongoApi.Entities;

namespace AspNetCoreMongoApi.Extensions.Mappers
{
    public static class TodoMapperExtensions
    {
        public static TodoResponse ToTodoResponse(this Todo todo, UserResponse user)
        {
            return new TodoResponse(todo.Id, todo.Title, todo.IsComplete, user, todo.CreatedAt, todo.UpdatedAt,todo.From, todo.To,  todo.Description);
        }

        public static Todo ToTodo(this CreateTodoRequest todo, string userId)
        {
            return new Todo() {
                Title = todo.Title,
                AuthorId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
                Description = todo.Description,
                From=new DateTime(todo.From!.Value.Ticks, DateTimeKind.Utc),
                To= new DateTime(todo.To!.Value.Ticks, DateTimeKind.Utc),
                IsComplete = todo.IsComplete};
        }
    }
}
