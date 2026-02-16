using AspNetCoreMongoApi.Options;
using FluentValidation;

namespace AspNetCoreMongoApi.Validators
{

    public class SeqOptionsValidator : AbstractValidator<SeqOptions>
    {
        public SeqOptionsValidator()
        {
            RuleFor(x => x.Url).NotEmpty().Must(text => Uri.TryCreate(text, UriKind.Absolute, out _)).WithMessage("Seq otlp exporter is required");
        }
    }
}
