using AspNetCoreMongoApi.Options;
using FluentValidation;

namespace AspNetCoreMongoApi.Validators.OptionsValidators
{

    public class SeqOptionsValidator : AbstractValidator<SeqOptions>
    { 
        public SeqOptionsValidator()
        {
            RuleFor(x => x.Url).NotEmpty().WithMessage("Seq otlp exporter is required");
        }
    }
}
