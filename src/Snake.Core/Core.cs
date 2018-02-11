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
        ISnakeWebhostBuilder With(Func<ISnakePart> f);
        IWebHost Build(string settingsFile, string assemblyName);
    }

    public class SnakeWebHostBuilder<TSettings> : ISnakeWebhostBuilder where TSettings: SnakeSettings, new()
    {
        private readonly string[] _args;
        private List<Func<ISnakePart>> _partsResolvers = new List<Func<ISnakePart>>();
        private TSettings _settings = new TSettings();

        private SnakeWebHostBuilder(string[] args) {
            _args = args;
        }

        public static ISnakeWebhostBuilder CreateDefaultBuilder(string[] args) {
            return new SnakeWebHostBuilder<TSettings>(args);
        }

        public ISnakeWebhostBuilder With(Func<ISnakePart> f)
        {
            _partsResolvers.Add(f);
            return this;
        }   
        
        public IWebHost Build(string settingsFile, string assemblyName) {
            IHostingEnvironment hostingEnvironment = null;

            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(settingsFile)
                .Build()
                .Bind(_settings);

            var parts = _partsResolvers
                .Select(pr => pr())
                .ToList();

            return 
                new WebHostBuilder()
                    .UseUrls(_settings.UseUrls)
                    .UseKestrel()
                    .UseIISIntegration()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .ConfigureAppConfiguration((hostingContext, config) => hostingEnvironment = hostingContext.HostingEnvironment)
                    .Configure((app) => parts.ForEach(p => p.Configure(app, hostingEnvironment)))
                    .ConfigureServices(services => parts.ForEach(p => p.ConfigureServices(services)))
                    .UseSetting(WebHostDefaults.ApplicationKey, assemblyName)
                    .Build();
        }
    }
}