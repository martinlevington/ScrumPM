using Microsoft.Extensions.Options;
using ScrumPm.Domain.Common.DependencyInjection;
using ScrumPm.Domain.Common.Persistence;

namespace ScrumPm.AspNetCore.Common.Configuration
{
    [ServiceRegistrationLocator(typeof(IConnectionStringResolver), typeof(DefaultConnectionStringResolver))]
    public class DefaultConnectionStringResolver : IConnectionStringResolver, ITransientDependency
    {
        protected DbConnectionOptions Options { get; }

        public DefaultConnectionStringResolver(IOptionsSnapshot<DbConnectionOptions> options)
        {
            Options = options.Value;
        }

        public virtual string Resolve(string connectionStringName = null)
        {
           
            //Get default value
            return Options.ConnectionStrings.Default;
        }
    }
}
