using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Snake.Core;
using System.Collections.Generic;

namespace Snake.Extensions.Mvc
{
    public static class MvcExtension {
        public static ISnakeWebhostBuilder WithMvc(this ISnakeWebhostBuilder builder) {
            return builder.With(() => new MvcPart());
        }
    }

    public class MvcPart : IApplicationPlugin
    {
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseMvc();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        public void BeforeBuild(IApplicationBuilder app, IHostingEnvironment env, IEnumerable<ServiceDescriptor> services) { }
    }
}