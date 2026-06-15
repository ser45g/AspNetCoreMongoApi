using AspNetCoreMongoApi.Contracts.Todos.Request;
using AspNetCoreMongoApi.Contracts.Todos.Response;
using AspNetCoreMongoApi.Entities;

namespace AspNetCoreMongoApi.Extensions.Mappers
{
    public static class TodoMapperExtensions
    {
        public static TodoResponse ToTodoResponse(this Todo todo)
        {
            return new TodoResponse(todo.Id, todo.Title, todo.IsComplete, todo.CreatedAt, todo.UpdatedAt,todo.From, todo.To,  todo.Description);
        }

        public static Todo ToTodo(this CreateTodoRequest todo)
        {
            return new Todo() { Title = todo.Title, CreatedAt = DateTime.UtcNow,UpdatedAt = null, Description = todo.Description, From=todo.From, To=todo.To, IsComplete = todo.IsComplete};
        }
    }
}
