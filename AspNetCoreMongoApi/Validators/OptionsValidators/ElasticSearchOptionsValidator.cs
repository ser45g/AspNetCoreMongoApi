using AspNetCoreMongoApi.Options;
using FluentValidation;

namespace AspNetCoreMongoApi.Validators.OptionsValidators
{
    public class ElasticSearchOptionsValidator:AbstractValidator<ElasticSearchOptions>
    {
        public ElasticSearchOptionsValidator() {
            RuleFor(o => o.Url).NotEmpty();

            RuleFor(o => o.LocalUsername).NotEmpty().When(o => o.CloudApiKey == null);
            RuleFor(o => o.LocalPassword).NotEmpty().When(o => o.CloudApiKey == null);

            RuleFor(o=>o.CloudApiKey).NotEmpty().When(o=>o.LocalPassword==null || o.LocalUsername==null );
        }
    }
}
