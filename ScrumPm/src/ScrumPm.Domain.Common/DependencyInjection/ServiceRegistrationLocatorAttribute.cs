using System;
using Microsoft.Extensions.DependencyInjection;

namespace ScrumPm.Domain.Common.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ServiceRegistrationLocatorAttribute : Attribute, IServiceRegistrationServiceTypesProvider
    {


        public Type Service { get; set; }

        public Type Implementation { get; set; }

        public ServiceLifetime Lifetime { get; set; }

        public ServiceRegistrationLocatorAttribute(Type service, Type implementation, ServiceLifetime  serviceLifetime = ServiceLifetime.Transient)
        {
            Service = service;
            Implementation = implementation;
            Lifetime = serviceLifetime;
        }

        public ServiceMapping GetServiceTypes()
        {
            return new ServiceMapping(Service, Implementation, Lifetime);
        }
    }
}
