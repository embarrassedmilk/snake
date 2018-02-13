# snake
[![Build status](https://ci.appveyor.com/api/projects/status/rxoy7ppxn0et2hna?svg=true)](https://ci.appveyor.com/project/embarrassedmilk/snake)

Allows to set up infrastructure for your ASP.NET Core application with just a few extension methods.

```c#
  SnakeWebHostBuilder<BaseSettings>
    .CreateDefaultBuilder(args)
    .WithMvc()
    .WithSwagger("Awesome app")
    .Build("appsettings.json", "Runner")
    .Run();
```

Available plugins atm:
* Mvc
* Swagger
* Autofac
* Serilog
