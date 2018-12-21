using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ScrumPm.Domain.Common.Extensions;

namespace ScrumPm.Domain.Common.DependencyInjection
{
    public static class AutoRegistrationHelper
    {

        public static IEnumerable<ServiceMapping> GetExposedServices(IServiceCollection services, Type type)
        {
            var typeInfo = type.GetTypeInfo();
            

            
            var customExposedServices = typeInfo
                .GetCustomAttributes()
                
                .Where(y =>
                    y.GetType().GetInterfaces().Contains(typeof(IServiceRegistrationServiceTypesProvider)))
                .OfType<IServiceRegistrationServiceTypesProvider>()
                .Select(p => p.GetServiceTypes())
                .ToList();

            return customExposedServices;
        }

        private static IEnumerable<ServiceMapping> GetDefaultExposedServices(IServiceCollection services, Type type)
        {
            var serviceTypes = new List<ServiceMapping>();

           

            foreach (var interfaceType in type.GetTypeInfo().GetInterfaces())
            {
                var interfaceName = interfaceType.Name;

                if (interfaceName.StartsWith("I"))
                {
                    interfaceName = interfaceName.Right(interfaceName.Length - 1);
                }

                if (type.Name.EndsWith(interfaceName))
                {
                    serviceTypes.Add(new ServiceMapping(type,interfaceType,ServiceLifetime.Transient));
                }
            }

            // todo determine if this is needed
            //var exposeActions = services.GetExposingActionList();
            //if (exposeActions.Any())
            //{
            //    var args = new OnServiceExposingContext(type, serviceTypes);
            //    foreach (var action in services.GetExposingActionList())
            //    {
            //        action(args);
            //    }
            //}

            return serviceTypes;
        }

    }
}
