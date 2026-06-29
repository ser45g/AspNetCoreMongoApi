using AspNetCoreMongoApi.Contracts.Todos.Request;
using AspNetCoreMongoApi.Contracts.Todos.Response;
using AspNetCoreMongoApi.Data;
using AspNetCoreMongoApi.Extensions.Mappers;
using AspNetCoreMongoApi.Helpers;
using AspNetCoreMongoApi.Options;
using FastEndpoints;
using Keycloak.AuthServices.Sdk.Admin;
using Keycloak.AuthServices.Sdk.Admin.Requests.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ZiggyCreatures.Caching.Fusion;

namespace AspNetCoreMongoApi.Endpoints.Todos
{
   
    public class GetTodoByIdEndpoint(AppDbContext context, IFusionCache hybridCache, IKeycloakUserClient keycloakUserClient, IOptions<KeycloakClientOptions> keycloakClientOptions) : Endpoint<GetByIdTodoRequest, TodoResponse>
    {
        public override void Configure()
        {
            Get(EndpointRoutes.Todos + "/{id:guid}");
        }

        public override async Task HandleAsync(GetByIdTodoRequest req, CancellationToken ct)
        {
            var todo = await hybridCache.GetOrSetAsync(CacheKeys.TodoById(req.Id), async cancellationToken => {
                return await context.Todos.FirstOrDefaultAsync(w => w.Id == req.Id, cancellationToken: cancellationToken);
            }, token: ct);

            if (todo == null)
            {
                await Send.NotFoundAsync(cancellation: ct);
                return;
            }
            
            var user = await keycloakUserClient.GetUserAsync(keycloakClientOptions.Value.Realm, todo.AuthorId, true, ct);

            if (user==null)
            {
                await Send.NotFoundAsync(cancellation: ct);
                return;
            }

            TodoResponse response = todo.ToTodoResponse(new Entities.User(user.Id!, $"{user.FirstName} {user.LastName}", user.Email!));

            await Send.OkAsync(response, ct);
        }
    }

}
