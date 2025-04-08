using System.Diagnostics;
using Microsoft.AspNetCore.HttpLogging;
using Radar.Infrastructure.Logging;
using Serilog;

namespace InvestList.Logging
{
    public static class LogExtension
    {
        /// <param name="customLoggerConfiguration">Give the ability to completely change default logger configuration</param>
        /// <param name="extendLoggerConfiguration">Add the ability to extend existing logger configuration</param>
        public static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder, Action<HostBuilderContext, IServiceProvider, LoggerConfiguration>? customLoggerConfiguration = null, Action<LoggerConfiguration>? extendLoggerConfiguration = null)
        {
            builder.Host.UseSerilog((ctx, services, loggerConfig) =>
            {
                LogConfigurator.Configure(loggerConfig, ctx.Configuration);
            });
            return builder;
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