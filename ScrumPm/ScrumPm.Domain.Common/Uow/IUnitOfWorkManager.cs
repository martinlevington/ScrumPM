using JetBrains.Annotations;

namespace ScrumPm.Domain.Common.Uow
{
    public interface IUnitOfWorkManager
    {
        [CanBeNull]
        IUnitOfWork Current { get; }

        [NotNull]
        IUnitOfWork CreateNew([NotNull] UnitOfWorkOptions options);

        [NotNull]
        IUnitOfWork Create([NotNull] UnitOfWorkOptions options);

        [NotNull]
        IUnitOfWork Create();

        [NotNull]
        IUnitOfWork Reserve([NotNull] string reservationName);

        void BeginReserved([NotNull] string reservationName, [NotNull] UnitOfWorkOptions options);

        bool TryBeginReserved([NotNull] string reservationName, [NotNull] UnitOfWorkOptions options);
    }
}
