using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Snake.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Snake.Extensions.Autofac
{
    public static class AutofacPartExtension
    {
        public static ISnakeWebhostBuilder WithAutofac(
            this ISnakeWebhostBuilder snakeWebhostBuilder, 
            IEnumerable<Module> modules, 
            Action<IContainer, IApplicationBuilder, IHostingEnvironment, IEnumerable<ServiceDescriptor>> afterBuild)
        {
            return snakeWebhostBuilder.With(() => new AutofacPart(modules, afterBuild));
        }
    }

    public class AutofacPart : IApplicationPlugin
    {
        private readonly IEnumerable<Module> _modules;
        private readonly Action<IContainer, IApplicationBuilder, IHostingEnvironment, IEnumerable<ServiceDescriptor>> _afterBuildAction;

        public AutofacPart(IEnumerable<Module> modules, Action<IContainer, IApplicationBuilder, IHostingEnvironment, IEnumerable<ServiceDescriptor>> afterBuildAction)
        {
            _modules = modules;
            _afterBuildAction = afterBuildAction;
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

            _afterBuildAction(container, app, env, services);
        }
    }
}
