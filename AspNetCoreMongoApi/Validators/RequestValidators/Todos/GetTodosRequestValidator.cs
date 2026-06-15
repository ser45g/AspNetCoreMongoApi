using AspNetCoreMongoApi.Contracts.Todos.Request;
using AspNetCoreMongoApi.Options;
using FastEndpoints;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace AspNetCoreMongoApi.Validators.RequestValidators.Todos
{
    public class GetTodosRequestValidator:Validator<GetTodosRequest>
    {
        public GetTodosRequestValidator(IOptions<PaginationOptions> options) {
            RuleFor(r => r.PageSize).InclusiveBetween(options.Value.MinPageSize, options.Value.MaxPageSize);
        }
    }
}
