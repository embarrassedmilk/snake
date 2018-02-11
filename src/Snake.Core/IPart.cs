using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Snake.Core
{
    public interface ISnakePart
    {
        void ConfigureServices(IServiceCollection services);
        void Configure(IApplicationBuilder app, IHostingEnvironment env);
    }
}