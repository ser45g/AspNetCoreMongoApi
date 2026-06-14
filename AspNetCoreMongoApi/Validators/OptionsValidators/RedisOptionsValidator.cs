using AspNetCoreMongoApi.Options;
using FluentValidation;

namespace AspNetCoreMongoApi.Validators.OptionsValidators
{
    public class RedisOptionsValidator:AbstractValidator<RedisOptions>
    {
        public RedisOptionsValidator() {
            RuleFor(r => r.Configuration).NotEmpty();
            RuleFor(r => r.Password).NotEmpty();
        }
    }
}
