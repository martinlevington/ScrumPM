using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

namespace ScrumPm.Domain.Common.DependencyInjection
{
    public class DefaultConventionalRegistrar : ConventionalRegistrarBase
    {
        public override void AddType(IServiceCollection services, Type type)
        {
            //if (IsConventionalRegistrationDisabled(type))
            //{
            //    return;
            //}

        
           

            //if (lifeTime == null)
            //{
            //    return;
            //}

            foreach (var serviceType in AutoRegistrationHelper.GetExposedServices(services, type))
            {

                var lifeTime = GetServiceLifetimeFromClassHierarcy(type);


                var serviceDescriptor = ServiceDescriptor.Describe(serviceType.Service, serviceType.Implementation, lifeTime.Value);

                //if (dependencyAttribute?.ReplaceServices == true)
                //{
                //    services.Replace(serviceDescriptor);
                //}
                //else if (dependencyAttribute?.TryRegister == true)
                //{
                //    services.TryAdd(serviceDescriptor);
                //}
                //else
                //{
                //    services.Add(serviceDescriptor);
                //}

                services.Add(serviceDescriptor);
            }
        }

        //protected virtual bool IsConventionalRegistrationDisabled(Type type)
        //{
        //    return type.IsDefined(typeof(DisableConventionalRegistrationAttribute), true);
        //}

        protected virtual DependencyAttribute GetDependencyAttributeOrNull(Type type)
        {
            return type.GetCustomAttribute<DependencyAttribute>(true);
        }

        //protected virtual ServiceLifetime? GetLifeTimeOrNull(Type type, [CanBeNull] DependencyAttribute dependencyAttribute)
        //{
        //    return dependencyAttribute?.Lifetime ?? GetServiceLifetimeFromClassHierarcy(type);
        //}

        protected virtual ServiceLifetime? GetServiceLifetimeFromClassHierarcy(Type type)
        {
            if (typeof(ITransientDependency).GetTypeInfo().IsAssignableFrom(type))
            {
                return ServiceLifetime.Transient;
            }

            if (type.GetInterfaces().Contains(typeof(ISingletonDependency)))
            {
                return ServiceLifetime.Singleton;
            }

            foreach (var a in type.GetInterfaces())
            {
                Console.WriteLine(a.FullName);
            }

            Console.WriteLine(typeof(ISingletonDependency).GetTypeInfo());


            if (typeof(ISingletonDependency).GetTypeInfo().IsAssignableFrom(type))
            {
                return ServiceLifetime.Singleton;
            }

            if (typeof(IScopedDependency).GetTypeInfo().IsAssignableFrom(type))
            {
                return ServiceLifetime.Scoped;
            }

            return ServiceLifetime.Transient;
        }
    }
}
