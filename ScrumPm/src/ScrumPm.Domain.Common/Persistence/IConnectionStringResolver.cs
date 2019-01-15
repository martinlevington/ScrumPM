using JetBrains.Annotations;

namespace ScrumPm.Domain.Common.Persistence
{
    public interface IConnectionStringResolver
    {
        [NotNull]
        string Resolve(string connectionStringName = null);
    }
}
