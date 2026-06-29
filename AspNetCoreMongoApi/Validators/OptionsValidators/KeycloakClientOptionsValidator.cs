using AspNetCoreMongoApi.Options;
using FluentValidation;

namespace AspNetCoreMongoApi.Validators.OptionsValidators
{
    public class KeycloakClientOptionsValidator:AbstractValidator<KeycloakClientOptions>
    {
        public KeycloakClientOptionsValidator() {
            RuleFor(a => a.ClientId).NotEmpty();
            RuleFor(a => a.Realm).NotEmpty();
            RuleFor(a => a.AuthServerUrl).NotEmpty();
            RuleFor(a => a.Secret).NotEmpty();

        }
    }
}
