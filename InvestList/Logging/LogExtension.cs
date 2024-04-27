using System.Diagnostics;
using Microsoft.AspNetCore.HttpLogging;
using Serilog;
using Serilog.Events;

namespace InvestList.Logging
{
    public static class LogExtension
    {
        /// <param name="customLoggerConfiguration">Give the ability to completely change default logger configuration</param>
        /// <param name="extendLoggerConfiguration">Add the ability to extend existing logger configuration</param>
        public static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder, Action<HostBuilderContext, IServiceProvider, LoggerConfiguration>? customLoggerConfiguration = null, Action<LoggerConfiguration>? extendLoggerConfiguration = null)
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Environment.CurrentDirectory = AppContext.BaseDirectory;
            builder.Host.UseSerilog(customLoggerConfiguration?? ConfigureDefaultLogger);
            builder.Services.AddHttpLogging(logging =>
            {
                logging.LoggingFields =
                    HttpLoggingFields.RequestBody |
                    HttpLoggingFields.RequestHeaders;
            });
            return builder;

            void ConfigureDefaultLogger(HostBuilderContext hostingContext, IServiceProvider services, LoggerConfiguration loggerConfiguration)
            {
                loggerConfiguration.ConfigureDefaultLogger(hostingContext.Configuration);
                extendLoggerConfiguration?.Invoke(loggerConfiguration);
            }
        }
        
        public static LoggerConfiguration ConfigureDefaultLogger(this LoggerConfiguration loggerConfiguration, IConfiguration configuration)
        {
            return loggerConfiguration
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .Enrich.WithClientIp();
        }
    }
}