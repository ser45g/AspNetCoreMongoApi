using AspNetCoreMongoApi.Contracts.Common.Response;
using AspNetCoreMongoApi.Contracts.Todos.Request;
using AspNetCoreMongoApi.Contracts.Todos.Response;
using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.Entities;
using AspNetCoreMongoApi.Extensions;
using AspNetCoreMongoApi.Extensions.Mappers;
using AspNetCoreMongoApi.Helpers;
using AspNetCoreMongoApi.Options;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;

namespace AspNetCoreMongoApi.Endpoints.Todos
{

    public class GetTodosEndpoint(ElasticsearchClient elasticsearchClient, IOptions<PaginationOptions> paginationOptions) : Endpoint<GetTodosRequest, CursorPaginationResponse<IEnumerable<TodoResponse>>>
    {
        public override void Configure()
        {
            Get(EndpointRoutes.Todos);
        }

        public override async Task HandleAsync(GetTodosRequest request, CancellationToken ct)
        {

            var pageSize = request.PageSize ?? paginationOptions.Value.DefaultPageSize;

            //var searchTodosResponse = await elasticsearchClient.SearchAsync<Todo>(t =>BuildQuery(t.Indices("todos"), request, pageSize));

            var query = (SearchRequestDescriptor<Todo> t) =>
            {
                t.Indices("todos").Query(q => 
                    q.Bool(b => 
                       BuildSearchQuery(b, request)
                    )).Size(pageSize+1)
                    .Sort(s => s.Field(GetKeySelector(request.SortColumn), request.SortAsc == false ? SortOrder.Desc : SortOrder.Asc));

                if (request.Cursor.HasValue)
                {
                    t=t.SearchAfter(new FieldValue[] { FieldValue.String(request.Cursor.ToString()!) });
                }
            };

            var searchTodosResponse = await elasticsearchClient.SearchAsync<Todo>(query);

            if (searchTodosResponse.IsValidResponse == false)
            {
                await Send.ErrorsAsync(cancellation: ct);
                return;
            }

            var todos = searchTodosResponse.Documents;
            var total = searchTodosResponse.Total;

            var todoResponses = todos.Select(t => t.ToTodoResponse()).ToList();

            Guid? cursor = null;

            if (todoResponses.Count == pageSize + 1)
            {
                var last = todoResponses.LastOrDefault();

                if (last != null)
                {
                    cursor = last.Id;
                    todoResponses.Remove(last);
                }
            }

            var response = new CursorPaginationResponse<IEnumerable<TodoResponse>>(cursor, todoResponses, todoResponses.Count, (int)total);

            await Send.OkAsync(response, ct);
        }

        private Expression<Func<Todo, object?>> GetKeySelector(string? sortColumn = null)
        {
            Expression<Func<Todo, object?>> keySelector = sortColumn?.ToLower() switch
            {
                "iscomplete" => w => w.IsComplete,
                "to" => w => w.To,
                "from" => w => w.From,
                "createdat" => w => w.CreatedAt,
                "updatedat" => w => w.UpdatedAt,
                _ => w => w.Id
            };
            return keySelector;
        }

        private BoolQueryDescriptor<Todo> BuildSearchQuery(BoolQueryDescriptor<Todo> boolQuery, GetTodosRequest request)
        {
            boolQuery.Must(m => m.MatchAll());

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                boolQuery = boolQuery.Should(
                    sh => sh.Match(f => f
                        .Field(t => t.Title)
                        .Query(request.SearchTerm)
                        .Fuzziness(new Fuzziness("2"))),
                    sh => sh.Match(f => f
                        .Field(t => t.Description)
                        .Query(request.SearchTerm)
                        .Fuzziness(new Fuzziness("2")))
                );
                boolQuery = boolQuery.MinimumShouldMatch(1);
            }

            if (request.IsComplete.HasValue)
            {
                boolQuery = boolQuery.Filter(f => f.Term(t => t.Field(ff => ff.IsComplete).Value(request.IsComplete.Value)));
            }

            if (request.MinFrom!=null)
            {
                boolQuery = boolQuery.Filter(f => f.Range(r => r.Date(d=>d.Field(t => t.From).Gte(request.MinFrom))));
            }
            if (request.MaxFrom != null)
            {
                boolQuery = boolQuery.Filter(f => f.Range(r => r.Date(d => d.Field(t => t.From).Lte(request.MaxFrom))));
            }

            if (request.MinTo != null)
            {
                boolQuery = boolQuery.Filter(f => f.Range(r => r.Date(d => d.Field(t => t.To).Gte(request.MinTo))));
            }
            if (request.MaxTo != null)
            {
                boolQuery = boolQuery.Filter(f => f.Range(r => r.Date(d => d.Field(t => t.To).Lte(request.MaxTo))));
            }

            if (request.MinCreatedAt != null)
            {
                boolQuery = boolQuery.Filter(f => f.Range(r => r.Date(d => d.Field(t => t.CreatedAt).Gte(request.MinCreatedAt))));
            }
            if (request.MaxCreatedAt != null)
            {
                boolQuery = boolQuery.Filter(f => f.Range(r => r.Date(d => d.Field(t => t.CreatedAt).Lte(request.MaxCreatedAt))));
            }

            if (request.MinUpdatedAt != null)
            {
                boolQuery = boolQuery.Filter(f => f.Range(r => r.Date(d => d.Field(t => t.UpdatedAt).Gte(request.MinUpdatedAt))));
            }
            if (request.MaxUpdatedAt != null)
            {
                boolQuery = boolQuery.Filter(f => f.Range(r => r.Date(d => d.Field(t => t.UpdatedAt).Lte(request.MaxUpdatedAt))));
            }

            return boolQuery;
        }

    }
}
