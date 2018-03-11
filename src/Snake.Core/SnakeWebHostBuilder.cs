using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Snake.Core
{
    public interface ISnakeWebhostBuilder<TSettings> where TSettings : BaseSettings, new()
    {
        ISnakeWebhostBuilder<TSettings> With(Func<TSettings, IApplicationPlugin> f);
        IWebHost Build(string settingsFile, string assemblyName);
    }

    public class SnakeWebHostBuilder<TSettings> : ISnakeWebhostBuilder<TSettings> where TSettings: BaseSettings, new()
    {
        private readonly string[] _args;
        private List<Func<TSettings, IApplicationPlugin>> _pluginResolvers = new List<Func<TSettings, IApplicationPlugin>>();
        private TSettings _settings = new TSettings();

        private SnakeWebHostBuilder(string[] args) {
            _args = args;
        }

        public static ISnakeWebhostBuilder<TSettings> CreateDefaultBuilder(string[] args) {
            return new SnakeWebHostBuilder<TSettings>(args);
        }

        public ISnakeWebhostBuilder<TSettings> With(Func<TSettings, IApplicationPlugin> f)
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
                .Select(pr => pr(_settings))
                .ToList();

            return
                new WebHostBuilder()
                    .UseUrls(_settings.UseUrls)
                    .UseKestrel()
                    .UseIISIntegration()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .ConfigureAppConfiguration((hostingContext, config) => hostingEnvironment = hostingContext.HostingEnvironment)
                    .ConfigureServices(serviceCollection =>
                    {
                        plugins.ForEach(p => p.ConfigureServices(serviceCollection));
                        services = serviceCollection;
                    })
                    .Configure(app => 
                    {
                        plugins.ForEach(p => p.Configure(app, hostingEnvironment));
                        plugins.ForEach(p => p.BeforeBuild(app, hostingEnvironment, services));
                    })
                    .UseSetting(WebHostDefaults.ApplicationKey, assemblyName)
                    .Build();
        }
    }
}