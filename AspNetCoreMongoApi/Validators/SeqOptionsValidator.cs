using AspNetCoreMongoApi.Options;
using FluentValidation;

namespace AspNetCoreMongoApi.Validators
{

    public class SeqOptionsValidator : AbstractValidator<SeqOptions>
    { 
        public SeqOptionsValidator()
        {
            RuleFor(x => x.Url).NotEmpty().WithMessage("Seq otlp exporter is required");
        }
        //Hello, I'm Mr. Frog. Fuck you
    }
}
