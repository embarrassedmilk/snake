using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Snake.Core;

namespace Snake.Extensions.Serilog
{
    public static class SerilogPartExtension
    {
        public static ISnakeWebhostBuilder WithSerilog(this ISnakeWebhostBuilder snakeWebhostBuilder)
        {
            return snakeWebhostBuilder.With(() => new SerilogPart());
        } 
    }

    public class SerilogPart : ISnakePart
    {
        public SerilogPart()
        {
            Log.Logger = new LoggerConfiguration()
              .Enrich.FromLogContext()
              .WriteTo.Console()
              .CreateLogger();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(loggingBuilder =>  loggingBuilder.AddSerilog(dispose: true));
        }
    }
}
