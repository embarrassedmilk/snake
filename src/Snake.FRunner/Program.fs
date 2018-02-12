open Snake.Core
open Snake.Extensions.Mvc
open Snake.Extensions.Swagger
open Snake.Extensions.Serilog
open Microsoft.AspNetCore.Hosting
open Serilog

let getLoggerConfiguration = 
    LoggerConfiguration()
        .Enrich
        .FromLogContext()
        .WriteTo.Console()

[<EntryPoint>]
let main argv =
    SnakeWebHostBuilder<BaseSettings>
        .CreateDefaultBuilder(argv)
        .WithMvc()
        .WithSwagger("F# app")
        .WithSerilog(getLoggerConfiguration)
        .Build("appsettings.json", "Snake.FRunner")
        .Run()
    0