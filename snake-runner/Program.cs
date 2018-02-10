using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Snake.Core;
using Snake.Extensions;
using System.Reflection;

namespace snake_runner
{
    class Program
    {
        static void Main(string[] args)
        {
            SnakeWebHostBuilder<SnakeSettings>
                .CreateDefaultBuilder(args)
                .WithMvc()
                .Build("appsettings.json", typeof(Program).GetTypeInfo().Assembly.FullName)
                .Run();
        }
    }
}
