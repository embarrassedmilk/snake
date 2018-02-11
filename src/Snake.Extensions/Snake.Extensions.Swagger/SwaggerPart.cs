using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Snake.Core;
using Swashbuckle.AspNetCore.Swagger;

namespace Snake.Extensions {
    public static class SwaggerExtension {
        public static ISnakeWebhostBuilder WithSwagger(this ISnakeWebhostBuilder builder, string appName) {
            return builder.With<SwaggerPart>(() => new SwaggerPart(appName));
        }
    }

    public class SwaggerPart : ISnakePart
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
    }
}