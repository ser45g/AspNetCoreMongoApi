using AspNetCoreMongoApi.Contracts.Common.Response;
using AspNetCoreMongoApi.Contracts.Todos.Request;
using AspNetCoreMongoApi.Contracts.Todos.Response;
using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.Entities;
using AspNetCoreMongoApi.Extensions;
using AspNetCoreMongoApi.Extensions.Mappers;
using AspNetCoreMongoApi.Helpers;
using AspNetCoreMongoApi.Options;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;

namespace AspNetCoreMongoApi.Endpoints.Todos
{

    public class GetTodosEndpoint(MongoDbContext dbContext, IOptions<PaginationOptions> paginationOptions) : Endpoint<GetTodosRequest, CursorPaginationResponse<IEnumerable<TodoResponse>>>
    {
        public override void Configure()
        {
            Get(EndpointRoutes.Todos);
        }

        public override async Task HandleAsync(GetTodosRequest request, CancellationToken ct)
        {
            var startWith = request.Cursor ?? Guid.Empty;
            var pageSize = request.PageSize ?? paginationOptions.Value.DefaultPageSize;

            IQueryable<Todo> todosQuery = dbContext.Todos.AsNoTracking();

            todosQuery = todosQuery.Where(w => w.Id >= startWith);

            todosQuery = todosQuery.AddFilters(request.MinTo, request.MaxTo, request.MinFrom, request.MaxFrom, request.MinCreatedAt, request.MaxCreatedAt, request.MinUpdatedAt, request.MaxUpdatedAt, request.SearchTerm);

            var keySelector = GetKeySelector(request.SortColumn);

            todosQuery = request.SortAsc == true ? todosQuery.OrderBy(keySelector) : todosQuery.OrderByDescending(keySelector);

            int totalCount = await todosQuery.CountAsync();

            var todos = await todosQuery.Take(pageSize + 1).ToListAsync(ct);

            Guid? cursor = null;

            if (todos.Count == pageSize + 1)
            {
                var last = todos.LastOrDefault();

                if (last != null)
                {
                    cursor = last.Id;
                    todos.Remove(last);
                }
            }

            var todosResponse = todos.Select(todo => todo.ToTodoResponse());

            var response = new CursorPaginationResponse<IEnumerable<TodoResponse>>(cursor, todosResponse, todos.Count, totalCount);

            await Send.OkAsync(response, ct);
        }

        private Expression<Func<Todo, object?>> GetKeySelector(string? sortColumn = null)
        {
            Expression<Func<Todo, object?>> keySelector = sortColumn?.ToLower() switch
            {
                "title" => w => w.Title,
                "isComplete" => w => w.IsComplete,
                "to" => w => w.To,
                "from" => w => w.From,
                "createdAt" => w => w.CreatedAt,
                "updatedAt" => w => w.UpdatedAt,
                _ => w => w.Id
            };
            return keySelector;
        }

    }
}
