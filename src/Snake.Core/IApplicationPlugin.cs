using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Snake.Core
{
    public interface IApplicationPlugin
    {
        void ConfigureServices(IServiceCollection services);
        void Configure(IApplicationBuilder app, IHostingEnvironment env);
        void BeforeBuild(IApplicationBuilder app, IHostingEnvironment env, IEnumerable<ServiceDescriptor> services);
    }
}