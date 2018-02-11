using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Snake.Core
{
    public interface ISnakeWebhostBuilder
    {
        ISnakeWebhostBuilder With(Func<IApplicationPlugin> f);
        IWebHost Build(string settingsFile, string assemblyName);
    }

    public class SnakeWebHostBuilder<TSettings> : ISnakeWebhostBuilder where TSettings: BaseSettings, new()
    {
        private readonly string[] _args;
        private List<Func<IApplicationPlugin>> _pluginResolvers = new List<Func<IApplicationPlugin>>();
        private TSettings _settings = new TSettings();

        private SnakeWebHostBuilder(string[] args) {
            _args = args;
        }

        public static ISnakeWebhostBuilder CreateDefaultBuilder(string[] args) {
            return new SnakeWebHostBuilder<TSettings>(args);
        }

        public ISnakeWebhostBuilder With(Func<IApplicationPlugin> f)
        {
            _pluginResolvers.Add(f);
            return this;
        }   
        
        public IWebHost Build(string settingsFile, string assemblyName) {
            IHostingEnvironment hostingEnvironment = null;
            IEnumerable<ServiceDescriptor> services = null;

            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(settingsFile)
                .Build()
                .Bind(_settings);

            var plugins = _pluginResolvers
                .Select(pr => pr())
                .ToList();

            return
                new WebHostBuilder()
                    .UseUrls(_settings.UseUrls)
                    .UseKestrel()
                    .UseIISIntegration()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .ConfigureAppConfiguration((hostingContext, config) => hostingEnvironment = hostingContext.HostingEnvironment)
                    .Configure(app => 
                    {
                        plugins.ForEach(p => p.Configure(app, hostingEnvironment));
                        plugins.ForEach(p => p.BeforeBuild(app, hostingEnvironment, services));
                    })
                    .ConfigureServices(serviceCollection =>
                    {
                        plugins.ForEach(p => p.ConfigureServices(serviceCollection));
                        services = serviceCollection;
                    })
                    .UseSetting(WebHostDefaults.ApplicationKey, assemblyName)
                    .Build();
        }
    }
}