﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Json;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace My.Infrastructure.Logging;

public static class LoggingExtensions
{
    private static void UseMyLogger(this IWebHostEnvironment env, LoggingOptions options)
    {
        var assemblyName = Assembly.GetEntryAssembly()?.GetName().Name;

        var logsPath = Path.Combine(env.ContentRootPath, "logs");
        Directory.CreateDirectory(logsPath);
        var loggerConfiguration = new LoggerConfiguration();

        loggerConfiguration = loggerConfiguration
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithEnvironmentUserName()
            .Enrich.WithProperty("ProcessId", Environment.ProcessId)
            .Enrich.WithProperty("Assembly", assemblyName)
            .Enrich.WithProperty("Application", env.ApplicationName)
            .Enrich.WithProperty("EnvironmentName", env.EnvironmentName)
            .Enrich.WithProperty("ContentRootPath", env.ContentRootPath)
            .Enrich.WithProperty("WebRootPath", env.WebRootPath)
            .Enrich.WithExceptionDetails()
            .Filter.ByIncludingOnly((logEvent) =>
            {
                if (logEvent.Level >= options.File.MinimumLogEventLevel
                || logEvent.Level >= options.Elasticsearch.MinimumLogEventLevel)
                {
                    var sourceContext = logEvent.Properties.ContainsKey("SourceContext")
                         ? logEvent.Properties["SourceContext"].ToString()
                         : null;

                    var logLevel = GetLogLevel(sourceContext, options);

                    return logEvent.Level >= logLevel;
                }

                return false;
            })
            .WriteTo.File(Path.Combine(logsPath, "log.txt"),
                fileSizeLimitBytes: 10 * 1024 * 1024,
                rollOnFileSizeLimit: true,
                shared: true,
                flushToDiskInterval: TimeSpan.FromSeconds(1),
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{SourceContext}] [TraceId: {TraceId}] [MachineName: {MachineName}] [ProcessId: {ProcessId}] {Message:lj}{NewLine}{Exception}",
                restrictedToMinimumLevel: options.File.MinimumLogEventLevel);

        if (options.Elasticsearch != null && options.Elasticsearch.IsEnabled)
        {
            loggerConfiguration = loggerConfiguration
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(options.Elasticsearch.Host))
                {
                    MinimumLogEventLevel = options.Elasticsearch.MinimumLogEventLevel,
                    AutoRegisterTemplate = true,
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
                    IndexFormat = options.Elasticsearch.IndexFormat + "-{0:yyyy.MM.dd}",
                    // BufferBaseFilename = Path.Combine(env.ContentRootPath, "logs", "buffer"),
                    InlineFields = true,
                    EmitEventFailure = EmitEventFailureHandling.WriteToFailureSink,
                    FailureSink = new FileSink(Path.Combine(logsPath, "elasticsearch-failures.txt"), new JsonFormatter(), null),
                });
        }

        Log.Logger = loggerConfiguration.CreateLogger();
    }

    private static LoggingOptions SetDefault(LoggingOptions options)
    {
        options ??= new LoggingOptions
        {
        };

        options.LogLevel ??= new Dictionary<string, string>();

        if (!options.LogLevel.ContainsKey("Default"))
        {
            options.LogLevel["Default"] = "Warning";
        }

        options.File ??= new FileOptions
        {
            MinimumLogEventLevel = Serilog.Events.LogEventLevel.Warning,
        };

        options.Elasticsearch ??= new ElasticsearchOptions
        {
            IsEnabled = false,
            MinimumLogEventLevel = Serilog.Events.LogEventLevel.Warning,
        };

        options.EventLog ??= new EventLogOptions
        {
            IsEnabled = false,
        };
        return options;
    }

    private static Serilog.Events.LogEventLevel GetLogLevel(string context, LoggingOptions options)
    {
        context = context.Replace("\"", string.Empty);
        string level = "Default";
        var matches = options.LogLevel.Keys.Where(k => context.StartsWith(k, StringComparison.OrdinalIgnoreCase));

        if (matches.Any())
        {
            level = matches.Max();
        }

        return (Serilog.Events.LogEventLevel)Enum.Parse(typeof(Serilog.Events.LogEventLevel), options.LogLevel[level], true);
    }

    public static IWebHostBuilder UseMyLogger(this IWebHostBuilder builder, Func<IConfiguration, LoggingOptions> logOptions)
    {
        builder.ConfigureLogging((context, logging) =>
        {
            logging.Configure(options =>
            {
                // options.ActivityTrackingOptions = ActivityTrackingOptions.SpanId | ActivityTrackingOptions.TraceId | ActivityTrackingOptions.ParentId;
            });

            logging.AddAzureWebAppDiagnostics();

            logging.AddSerilog();

            LoggingOptions options = SetDefault(logOptions(context.Configuration));

            if (options.EventLog != null && options.EventLog.IsEnabled && OperatingSystem.IsWindows())
            {
                logging.AddEventLog(new EventLogSettings
                {
                    LogName = options.EventLog.LogName,
                    SourceName = options.EventLog.SourceName,
                });
            }

            if (options?.ApplicationInsights?.IsEnabled ?? false)
            {
                logging.AddApplicationInsights(
                    configureTelemetryConfiguration: (config) =>
                    {
                        config.ConnectionString = options.ApplicationInsights.InstrumentationKey;
                    },
                    configureApplicationInsightsLoggerOptions: (options) => { });
            }

            if (options?.OpenTelemetry?.IsEnabled ?? false)
            {
                var resourceBuilder = ResourceBuilder.CreateDefault().AddService(options.OpenTelemetry.ServiceName);

                logging.AddOpenTelemetry(configure =>
                {
                    configure.SetResourceBuilder(resourceBuilder)
                    .AddOtlpExporter(otlpOptions =>
                    {
                        otlpOptions.Endpoint = new Uri(options.OpenTelemetry.Otlp.Endpoint);
                    });
                });
            }

            context.HostingEnvironment.UseMyLogger(options);
        });

        return builder;
    }

    public static IHostBuilder UseMyLogger(this IHostBuilder builder, Func<IConfiguration, LoggingOptions> logOptions)
    {
        builder.ConfigureLogging((context, logging) =>
        {
            logging.Configure(options =>
            {
                // options.ActivityTrackingOptions = ActivityTrackingOptions.SpanId | ActivityTrackingOptions.TraceId | ActivityTrackingOptions.ParentId;
            });

            logging.AddAzureWebAppDiagnostics();

            logging.AddSerilog();

            LoggingOptions options = SetDefault(logOptions(context.Configuration));

            if (options.EventLog != null && options.EventLog.IsEnabled && OperatingSystem.IsWindows())
            {
                logging.AddEventLog(new EventLogSettings
                {
                    LogName = options.EventLog.LogName,
                    SourceName = options.EventLog.SourceName,
                });
            }

            if (options?.ApplicationInsights?.IsEnabled ?? false)
            {
                logging.AddApplicationInsights(
                    configureTelemetryConfiguration: (config) =>
                    {
                        config.ConnectionString = options.ApplicationInsights.InstrumentationKey;
                    },
                    configureApplicationInsightsLoggerOptions: (options) => { });
            }

            if (options?.OpenTelemetry?.IsEnabled ?? false)
            {
                var resourceBuilder = ResourceBuilder.CreateDefault().AddService(options.OpenTelemetry.ServiceName);

                logging.AddOpenTelemetry(configure =>
                {
                    configure.SetResourceBuilder(resourceBuilder)
                    .AddOtlpExporter(otlpOptions =>
                    {
                        otlpOptions.Endpoint = new Uri(options.OpenTelemetry.Otlp.Endpoint);
                    });
                });
            }

            context.HostingEnvironment.UseMyLogger(options);
        });

        return builder;
    }

    private static void UseMyLogger(this IHostEnvironment env, LoggingOptions options)
    {
        var assemblyName = Assembly.GetEntryAssembly()?.GetName().Name;

        var logsPath = Path.Combine(env.ContentRootPath, "logs");
        Directory.CreateDirectory(logsPath);
        var loggerConfiguration = new LoggerConfiguration();

        loggerConfiguration = loggerConfiguration
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithEnvironmentUserName()
            .Enrich.WithProperty("ProcessId", Environment.ProcessId)
            .Enrich.WithProperty("Assembly", assemblyName)
            .Enrich.WithProperty("Application", env.ApplicationName)
            .Enrich.WithProperty("EnvironmentName", env.EnvironmentName)
            .Enrich.WithProperty("ContentRootPath", env.ContentRootPath)
            .Enrich.WithExceptionDetails()
            .Filter.ByIncludingOnly((logEvent) =>
            {
                if (logEvent.Level >= options.File.MinimumLogEventLevel
                || logEvent.Level >= options.Elasticsearch.MinimumLogEventLevel)
                {
                    var sourceContext = logEvent.Properties.ContainsKey("SourceContext")
                         ? logEvent.Properties["SourceContext"].ToString()
                         : null;

                    var logLevel = GetLogLevel(sourceContext, options);

                    return logEvent.Level >= logLevel;
                }

                return false;
            })
            .WriteTo.File(Path.Combine(logsPath, "log.txt"),
                fileSizeLimitBytes: 10 * 1024 * 1024,
                rollOnFileSizeLimit: true,
                shared: true,
                flushToDiskInterval: TimeSpan.FromSeconds(1),
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{SourceContext}] [TraceId: {TraceId}] [MachineName: {MachineName}] [ProcessId: {ProcessId}] {Message:lj}{NewLine}{Exception}",
                restrictedToMinimumLevel: options.File.MinimumLogEventLevel);

        if (options.Elasticsearch != null && options.Elasticsearch.IsEnabled)
        {
            loggerConfiguration = loggerConfiguration
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(options.Elasticsearch.Host))
                {
                    MinimumLogEventLevel = options.Elasticsearch.MinimumLogEventLevel,
                    AutoRegisterTemplate = true,
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
                    IndexFormat = options.Elasticsearch.IndexFormat + "-{0:yyyy.MM.dd}",
                    // BufferBaseFilename = Path.Combine(env.ContentRootPath, "logs", "buffer"),
                    InlineFields = true,
                    EmitEventFailure = EmitEventFailureHandling.WriteToFailureSink,
                    FailureSink = new FileSink(Path.Combine(logsPath, "elasticsearch-failures.txt"), new JsonFormatter(), null),
                });
        }

        Log.Logger = loggerConfiguration.CreateLogger();
    }
}
