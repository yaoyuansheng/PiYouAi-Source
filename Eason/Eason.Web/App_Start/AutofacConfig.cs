using Autofac;
using Autofac.Integration.Mvc;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Eason.Web.App_Start
{
    public class AutofacConfig
    {
        public static void RegisterIOC()
        {
            var builder = new ContainerBuilder();
            //builder.RegisterType<Eason.Redis.Session.RedisSession>().AsSelf();
            builder.RegisterType<EntityFramework.EasonEntities>().AsSelf();
            var DbAssembly = Assembly.Load("Eason.EntityFramework");
            var CasAssembly = Assembly.Load("Eason.Application");
            builder.RegisterAssemblyTypes(DbAssembly).Where(m => m.Namespace.StartsWith("Eason.EntityFramework.Repositories") && m.Name.EndsWith("Repository")).AsSelf();
           
            builder.RegisterAssemblyTypes(CasAssembly).Where(m => m.Name.EndsWith("Service")).AsImplementedInterfaces().PropertiesAutowired();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}