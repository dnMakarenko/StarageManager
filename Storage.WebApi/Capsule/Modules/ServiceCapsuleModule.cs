using Autofac;
using Storage.WebApi.Core;
using Storage.WebApi.Services;
using System.Reflection;

namespace Storage.WebApi.Capsule.Modules
{
    public class ServiceCapsuleModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load("Storage.WebApi")).
                Where(_ => _.Name.EndsWith("Service")).
                AsImplementedInterfaces().
                InstancePerLifetimeScope();

            builder.RegisterType(typeof(ProductService)).As(typeof(IProductService)).InstancePerDependency();
            builder.RegisterType(typeof(BasketService)).As(typeof(IBasketService)).InstancePerDependency();
                //.RegisterGeneric(typeof(BasketService)).As(typeof(IBasketService)).InstancePerDependency();
        }

    }
}