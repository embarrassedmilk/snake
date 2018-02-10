using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Snake.Core
{
    public interface ISnakeWebhostBuilder
    {
        ISnakeWebhostBuilder With<T>() where T: IPart, new();
        IWebHost Build(string settingsFile, string assemblyName);
    }

    public class SnakeWebHostBuilder<TSettings> : ISnakeWebhostBuilder where TSettings: SnakeSettings, new()
    {
        private readonly string[] _args;
        private IList<Type> _parts = new List<Type>();
        private TSettings _settings = new TSettings();

        private SnakeWebHostBuilder(string[] args) {
            _args = args;
        }

        public static ISnakeWebhostBuilder CreateDefaultBuilder(string[] args) {
            return new SnakeWebHostBuilder<TSettings>(args);
        }

        public ISnakeWebhostBuilder With<T>() where T : IPart, new()
        {
            _parts.Add(typeof(T));
            return this;
        }

        public IWebHost Build(string settingsFile, string assemblyName) {
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(settingsFile)
                .Build()
                .Bind(_settings);

            var instancesOfParts = _parts
                .Select(p => (IPart)Activator.CreateInstance(p))
                .ToList();

            IHostingEnvironment hostingEnvironment = null;
            return 
                new WebHostBuilder()
                    .UseUrls(_settings.UseUrls)
                    .UseKestrel()
                    .UseIISIntegration()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .ConfigureAppConfiguration((hostingContext, config) => hostingEnvironment = hostingContext.HostingEnvironment)
                    .Configure((app) => instancesOfParts.ForEach(p => p.Configure(app, hostingEnvironment)))
                    .ConfigureServices(services => instancesOfParts.ForEach(p => p.ConfigureServices(services)))
                    .UseSetting(WebHostDefaults.ApplicationKey, assemblyName)
                    .Build();
        }
    }
}