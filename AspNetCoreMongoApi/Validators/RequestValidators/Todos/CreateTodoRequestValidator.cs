using AspNetCoreMongoApi.Contracts.Todos.Request;
using FastEndpoints;
using FluentValidation;

namespace AspNetCoreMongoApi.Validators.RequestValidators.Todos
{
    public class CreateTodoRequestValidator:Validator<CreateTodoRequest>
    {
        public CreateTodoRequestValidator() {
            RuleFor(t=>t.From).LessThanOrEqualTo(t=>t.To).NotNull();
            RuleFor(t=>t.To).GreaterThanOrEqualTo(t=>t.From).NotNull();

            RuleFor(t => t.Title).NotEmpty().Length(5, 100);
            RuleFor(t => t.Description).Length(5, 250);

        }
    }
}
