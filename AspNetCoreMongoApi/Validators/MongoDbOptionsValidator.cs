using AspNetCoreMongoApi.Options;
using FluentValidation;

namespace AspNetCoreMongoApi.Validators
{

    public class MongoDbOptionsValidator : AbstractValidator<MongoDbOptions>
    {
        public MongoDbOptionsValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("MaxRetries must be greater than 0");
            RuleFor(x => x.Url).NotEmpty().Must(text=>Uri.TryCreate(text, UriKind.Absolute, out _)).WithMessage("Database Connection is required");
        }
    }
}
