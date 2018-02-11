using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Snake.Core;

namespace Snake.Extensions
{
    public static class MvcExtension {
        public static ISnakeWebhostBuilder WithMvc(this ISnakeWebhostBuilder builder) {
            return builder.With<MvcPart>();
        }
    }

    public class MvcPart : ISnakePart
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
    }
}