using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace ScrumPm.Domain.Common.Uow
{
    public interface IUnitOfWork : IDatabaseApiContainer, ITransactionApiContainer, IDisposable
    {
        Guid Id { get; }

        event EventHandler<UnitOfWorkEventArgs> Disposed;

        event EventHandler<UnitOfWorkFailedEventArgs> Failed;

        IUnitOfWorkOptions Options { get; }

        IUnitOfWork Outer { get; }

        bool IsReserved { get; }

        bool IsDisposed { get; }

        bool IsCompleted { get; }

        string ReservationName { get; }

        void SetOuter([CanBeNull] IUnitOfWork outer);

        void Initialize([NotNull] UnitOfWorkOptions options);

        void Reserve([NotNull] string reservationName);

        void SaveChanges();

        Task SaveChangesAsync(CancellationToken cancellationToken = default);

        void Complete();

        Task CompleteAsync(CancellationToken cancellationToken = default);

        void Rollback();

        Task RollbackAsync(CancellationToken cancellationToken = default);

        void OnCompleted(Func<Task> handler);

        bool IsReservedFor(string reservationName);
    }
}
