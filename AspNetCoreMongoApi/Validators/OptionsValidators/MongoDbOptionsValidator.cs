using AspNetCoreMongoApi.Options;
using FluentValidation;

namespace AspNetCoreMongoApi.Validators.OptionsValidators
{

    public class MongoDbOptionsValidator : AbstractValidator<MongoDbOptions>
    {
        public MongoDbOptionsValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
            RuleFor(x => x.Url).NotEmpty().WithMessage("MongoDb url is required");
        }
    }
}
