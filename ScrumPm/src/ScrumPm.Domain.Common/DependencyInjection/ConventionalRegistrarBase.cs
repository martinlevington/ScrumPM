using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace ScrumPm.Domain.Common.DependencyInjection
{
    public abstract class ConventionalRegistrarBase
    {


        public virtual void LoadAllFromAssembly(IServiceCollection services)
        {
            var allAssemblies = new List<Assembly>();
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            foreach (var dll in Directory.GetFiles(path, "*.dll"))
            {
                allAssemblies.Add(Assembly.LoadFile(dll));
            }

            var types = new List<Type>();
            foreach (var assembly in allAssemblies)
            {
                AddAssembly(services, assembly);
            }

        }

        public virtual void LoadFromAssembly(IServiceCollection serviceCollection, Assembly assembly)
        {
            AddAssembly(serviceCollection, assembly);
        }
            
        public virtual void LoadFromAssembly(IServiceCollection serviceCollection, Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                AddAssembly(serviceCollection, assembly);
            }
        }


        public virtual void AddAssembly(IServiceCollection services, Assembly assembly)
        {
            var types = assembly
                .GetTypes()
                .Where(
                    type => type.IsClass &&
                            !type.IsAbstract &&
                            !type.IsGenericType
                ).ToArray();

            AddTypes(services, types);
        }

        public virtual void AddTypes(IServiceCollection services, params Type[] types)
        {
            foreach (var type in types)
            {
                AddType(services, type);
            }
        }

        public abstract void AddType(IServiceCollection services, Type type);

    }
}
