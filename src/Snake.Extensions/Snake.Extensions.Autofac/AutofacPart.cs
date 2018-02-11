using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Snake.Core;
using System.Collections.Generic;
using System.Linq;

namespace Snake.Extensions.Autofac
{
    public static class AutofacPartExtension
    {
        public static ISnakeWebhostBuilder WithAutofac(this ISnakeWebhostBuilder snakeWebhostBuilder, IEnumerable<Module> modules)
        {
            return snakeWebhostBuilder.With(() => new AutofacPart(modules));
        }
    }

    public class AutofacPart : IApplicationPlugin
    {
        private readonly IEnumerable<Module> _modules;

        public AutofacPart(IEnumerable<Module> modules)
        {
            _modules = modules;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env) { }

        public void ConfigureServices(IServiceCollection services) { }

        public void ConfigureContainer(ContainerBuilder builder) 
            => _modules.ToList().ForEach(m => builder.RegisterModule(m));

        public void BeforeBuild(IApplicationBuilder app, IHostingEnvironment env, IEnumerable<ServiceDescriptor> services)
        {
            var containerBuilder = new ContainerBuilder();
            ConfigureContainer(containerBuilder);
            containerBuilder.Populate(services);
            var container = containerBuilder.Build();
            app.ApplicationServices = new AutofacServiceProvider(container);
        }
    }
}
