using My.Infrastructure.Monitoring.AzureApplicationInsights;
using My.Infrastructure.Monitoring.MiniProfiler;
using My.Infrastructure.Monitoring.OpenTelemetry;

namespace My.Infrastructure.Monitoring;

public class MonitoringOptions
{
    public MiniProfilerOptions MiniProfiler { get; set; }

    public AzureApplicationInsightsOptions AzureApplicationInsights { get; set; }

    public OpenTelemetryOptions OpenTelemetry { get; set; }
}
