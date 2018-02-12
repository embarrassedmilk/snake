using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
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
                .WithAutofac(GetModules(), AfterBuild)
                .WithMvc()
                .WithSerilog(GetLoggerConfiguration())
                .WithSwagger("Dont touch my bread, government")
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

        private static void AfterBuild(IContainer container, IApplicationBuilder app, IHostingEnvironment env, IEnumerable<ServiceDescriptor> services) { }
    }
}
