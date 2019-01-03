using System;
using JetBrains.Annotations;
using ScrumPm.Domain.Common.DependencyInjection;

namespace ScrumPm.Domain.Common.Uow
{
    public interface IDatabaseApiContainer 
    {
        [CanBeNull]
        IDatabaseApi FindDatabaseApi([NotNull] string key);

        void AddDatabaseApi([NotNull] string key, [NotNull] IDatabaseApi api);

        [NotNull]
        IDatabaseApi GetOrAddDatabaseApi([NotNull] string key, [NotNull] Func<IDatabaseApi> factory);
    }
}
