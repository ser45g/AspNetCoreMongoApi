using AspNetCoreMongoApi.Contracts.Common.Response;

namespace AspNetCoreMongoApi.Contracts.Todos.Response
{
    public record class TodoResponse(Guid Id, string Title, bool IsComplete, UserResponse User, DateTime CreatedAt, DateTime? UpdatedAt, DateTime? From, DateTime? To, string? Description);
   
}
