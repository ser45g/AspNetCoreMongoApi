namespace AspNetCoreMongoApi.Contracts.Todos.Request
{
    public record class UpdateTodoRequest(Guid Id, string Title, bool IsComplete, DateTime? From, DateTime? To, string? Description);

}
