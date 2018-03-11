using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Snake.Core;
using System.Collections.Generic;

namespace Snake.Extensions.Serilog
{
    public static class SerilogPartExtension
    {
        public static ISnakeWebhostBuilder<TSettings> WithSerilog<TSettings>(this ISnakeWebhostBuilder<TSettings> snakeWebhostBuilder, LoggerConfiguration loggerConfiguration) 
            where TSettings : BaseSettings, new()
        {
            return snakeWebhostBuilder.With((_) => new SerilogPart(loggerConfiguration));
        }
    }

    public class SerilogPart : IApplicationPlugin
    {
        public SerilogPart(LoggerConfiguration loggerConfiguration)
        {
            Log.Logger = loggerConfiguration.CreateLogger();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
        }

        public void BeforeBuild(IApplicationBuilder app, IHostingEnvironment env, IEnumerable<ServiceDescriptor> services) { }
    }
}
