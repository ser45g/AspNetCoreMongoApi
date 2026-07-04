using AspNetCoreMongoApi.Options;
using FluentValidation;

namespace AspNetCoreMongoApi.Validators.OptionsValidators
{
    public class MailOptionsValidator: AbstractValidator<MailOptions>
    {
        public MailOptionsValidator() {
            RuleFor(m => m.From).NotEmpty();
            RuleFor(m => m.DisplayName).NotEmpty();
            RuleFor(m => m.Host).NotEmpty();

            RuleFor(o => o.UserName).NotEmpty().When(o => o.IsAuthenticated);
            RuleFor(o => o.Password).NotEmpty().When(o => o.IsAuthenticated);

        }
    }
}
