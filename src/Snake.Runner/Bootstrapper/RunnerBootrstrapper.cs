using Autofac;
using temp.Controllers;

namespace Snake.Runner.Bootstrapper
{
    public class RunnerBootrstrapper : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ValuesRepository>().As<IValuesRepository>();
        }
    }
}
