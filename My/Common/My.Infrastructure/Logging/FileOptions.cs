using Serilog.Events;

namespace My.Infrastructure.Logging;

public class FileOptions
{
    public LogEventLevel MinimumLogEventLevel { get; set; }
}
