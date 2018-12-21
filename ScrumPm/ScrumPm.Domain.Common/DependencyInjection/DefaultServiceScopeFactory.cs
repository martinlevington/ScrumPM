using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using ScrumPm.Domain.Common.DependancyInjection;

namespace ScrumPm.Domain.Common.DependencyInjection
{
    [ServiceRegistrationLocator( 
        typeof(IHybridServiceScopeFactory),
        typeof(DefaultServiceScopeFactory)
    )]
    public class DefaultServiceScopeFactory : IHybridServiceScopeFactory, ITransientDependency
    {
        protected IServiceScopeFactory Factory { get; }

        public DefaultServiceScopeFactory(IServiceScopeFactory factory)
        {
            Factory = factory;
        }

        public IServiceScope CreateScope()
        {
            return Factory.CreateScope();
        }
    }
}
