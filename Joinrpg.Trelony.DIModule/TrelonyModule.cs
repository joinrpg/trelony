using Autofac;
using Joinrpg.Trelony.DataAccess.Repositories;

namespace Joinrpg.Trelony.DIModule
{
    public class TrelonyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CalendarRepository>().AsImplementedInterfaces().InstancePerDependency();
        }
    }
}
