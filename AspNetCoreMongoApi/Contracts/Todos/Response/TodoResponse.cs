namespace AspNetCoreMongoApi.Contracts.Todos.Response
{
    public record class TodoResponse(Guid Id, string Title, bool IsComplete, DateTime CreatedAt, DateTime? UpdatedAt, DateTime? From, DateTime? To, string? Description);
   
}
