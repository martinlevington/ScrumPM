using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace ScrumPm.Domain.Common.Uow
{
    public interface IUnitOfWork : IDatabaseApiContainer, ITransactionApiContainer, IDisposable
    {
        Guid Id { get; }



        IUnitOfWorkOptions Options { get; }


        void Initialize([NotNull] UnitOfWorkOptions options);


        void SaveChanges();

        Task SaveChangesAsync(CancellationToken cancellationToken = default);

        void Complete();

        Task CompleteAsync(CancellationToken cancellationToken = default);

        void Rollback();

        Task RollbackAsync(CancellationToken cancellationToken = default);

    

    }
}
