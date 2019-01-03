using System;
using Microsoft.Extensions.DependencyInjection;

namespace ScrumPm.Domain.Common.DependencyInjection
{
    public class ServiceMapping
    {
        public Type Service { get; set; }

        public Type Implementation { get; set; }

        public ServiceLifetime Lifetime { get; set; }

        public ServiceMapping(Type service, Type implementation, ServiceLifetime lifetime)
        {
            Service = service;
            Implementation = implementation;

            Lifetime = lifetime;
        }

        
    }
}
