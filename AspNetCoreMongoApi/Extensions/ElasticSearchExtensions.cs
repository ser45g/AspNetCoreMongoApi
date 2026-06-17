using AspNetCoreMongoApi.Options;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;

namespace AspNetCoreMongoApi.Extensions
{
    public static class ElasticSearchExtensions
    {
        public static IServiceCollection AddElasticSearchClient(this IServiceCollection services, ElasticSearchOptions options) {

            services.AddSingleton<ElasticsearchClient>(provider =>
            {
                var settings = new ElasticsearchClientSettings(new Uri(options.Url));

                if (!string.IsNullOrEmpty(options.CloudApiKey))
                {
                    settings.Authentication(new ApiKey(options.CloudApiKey));
                }
                else
                {
                    settings.Authentication(new BasicAuthentication(options.LocalUsername!, options.LocalPassword!));
                }

                if(!options.IsSslUsed)
                    settings.ServerCertificateValidationCallback((sender, cert, chain, sslPolicyErrors) => true);

                return new ElasticsearchClient(settings);
            });

            return services;

        }
    }
}
