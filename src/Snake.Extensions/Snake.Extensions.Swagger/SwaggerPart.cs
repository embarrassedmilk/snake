using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Snake.Core;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;

namespace Snake.Extensions.Swagger
{
    public static class SwaggerExtension {
        public static ISnakeWebhostBuilder<TSettings> WithSwagger<TSettings>(this ISnakeWebhostBuilder<TSettings> builder, string appName)
             where TSettings : BaseSettings, new()
        {
            return builder.With((_) => new SwaggerPart(appName));
        }
    }

    public class SwaggerPart : IApplicationPlugin
    {
        private readonly string _appName;

        public SwaggerPart(string appName) {
            _appName = appName;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", _appName);
            });
        }

        public void ConfigureServices(IServiceCollection services) 
            => services
                .AddSwaggerGen(c =>
                    {
                        c.SwaggerDoc("v1", new Info { Title = _appName, Version = "v1" });
                    });

        public void BeforeBuild(IApplicationBuilder app, IHostingEnvironment env, IEnumerable<ServiceDescriptor> services) { }
    }
}