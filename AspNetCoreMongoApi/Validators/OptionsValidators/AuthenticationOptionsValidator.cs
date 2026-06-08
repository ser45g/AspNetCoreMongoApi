using AspNetCoreMongoApi.Options;
using FluentValidation;

namespace AspNetCoreMongoApi.Validators.OptionsValidators
{
    public class AuthenticationOptionsValidator:AbstractValidator<AuthenticationOptions>
    {
        public AuthenticationOptionsValidator() {
            RuleFor(a => a.ClientId).NotEmpty();
            RuleFor(a => a.ValidIssuer).NotEmpty();
            RuleFor(a => a.MetadataAddress).NotEmpty();
            RuleFor(a => a.RefreshUrl).NotEmpty();
            RuleFor(a => a.Audience).NotEmpty();
            RuleFor(a => a.AuthorizationUrl).NotEmpty();
            RuleFor(a => a.TokenUrl).NotEmpty();
        }
    }
}
