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
            RuleFor(x => x.Url).NotEmpty().WithMessage("Database Connection is required");
        }
    }
}
