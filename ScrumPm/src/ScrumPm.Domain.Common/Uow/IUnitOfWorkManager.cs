using JetBrains.Annotations;

namespace ScrumPm.Domain.Common.Uow
{
    public interface IUnitOfWorkManager<out T>
    {
        [CanBeNull]
        T Current { get; }

        [NotNull]
        T CreateNew([NotNull] UnitOfWorkOptions options);

        [NotNull]
        T Create([NotNull] UnitOfWorkOptions options);

        [NotNull]
        T Create();

      
    }
}
