using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Snake.Core;
using Snake.Extensions.Autofac;
using Snake.Extensions.Mvc;
using Snake.Extensions.Serilog;
using Snake.Extensions.Swagger;
using Snake.Runner.Bootstrapper;
using System.Collections.Generic;
using System.Reflection;

namespace snake_runner
{
    class Program
    {
        static void Main(string[] args)
        {
            SnakeWebHostBuilder<BaseSettings>
                .CreateDefaultBuilder(args)
                .WithSerilog(GetLoggerConfiguration())
                .WithAutofac(GetModules())
                .WithSwagger("Dont touch my bread, government")
                .WithMvc()
                .Build("appsettings.json", typeof(Program).GetTypeInfo().Assembly.FullName)
                .Run();
        }

        private static LoggerConfiguration GetLoggerConfiguration() 
            => new LoggerConfiguration()
                   .Enrich
                   .FromLogContext()
                   .WriteTo.Console();

        private static IEnumerable<Autofac.Module> GetModules()
            => new[] { new RunnerBootrstrapper() };
    }
}
