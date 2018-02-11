﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Snake.Core;
using Snake.Extensions;
using System.Reflection;
using Snake.Extensions.Serilog;

namespace snake_runner
{
    class Program
    {
        static void Main(string[] args)
        {
            SnakeWebHostBuilder<SnakeSettings>
                .CreateDefaultBuilder(args)
                .WithMvc()
                .WithSerilog()
                .WithSwagger("Dont touch my bread, government")
                .Build("appsettings.json", typeof(Program).GetTypeInfo().Assembly.FullName)
                .Run();
        }
    }
}
