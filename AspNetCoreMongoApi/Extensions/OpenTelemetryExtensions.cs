using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace AspNetCoreMongoApi.Extensions
{
    public static class OpenTelemetryExtensions
    {
        public static IHostApplicationBuilder AddAppOpenTelemetryLogging(this IHostApplicationBuilder builder) {

            builder.Services.AddLogging();

            builder.Services.AddOpenTelemetry().ConfigureResource(resource =>
            {
                resource.AddService("WeatherForecast");
            }).WithMetrics(metrics =>
            {
                metrics.AddAspNetCoreInstrumentation().AddHttpClientInstrumentation();

                metrics.AddOtlpExporter();
            }).WithTracing(tracing =>
            {
                tracing.AddAspNetCoreInstrumentation().AddHttpClientInstrumentation().AddEntityFrameworkCoreInstrumentation();

                tracing.AddOtlpExporter();
            });

            builder.Logging.AddOpenTelemetry(logging => logging.AddOtlpExporter());

            return builder;
        }
    }
}
