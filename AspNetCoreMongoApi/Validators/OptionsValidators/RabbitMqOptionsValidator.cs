using AspNetCoreMongoApi.Options;
using FluentValidation;

namespace AspNetCoreMongoApi.Validators.OptionsValidators
{
    public class RabbitMqOptionsValidator: AbstractValidator<RabbitMqOptions>
    {
        public RabbitMqOptionsValidator() {
            RuleFor(r => r.Url).NotEmpty();
            RuleFor(r => r.Username).NotEmpty();
            RuleFor(r => r.Password).NotEmpty();
        }
    }
}
