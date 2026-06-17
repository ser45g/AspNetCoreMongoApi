using AspNetCoreMongoApi.Options;
using FluentValidation;

namespace AspNetCoreMongoApi.Validators.OptionsValidators
{

    public class DbOptionsValidator : AbstractValidator<DbOptions>
    {
        public DbOptionsValidator()
        {
            RuleFor(x => x.ConnectionString).NotEmpty();
        }
    }
}
