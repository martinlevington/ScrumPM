using System.Threading;
using System.Threading.Tasks;

namespace ScrumPm.Domain.Common.Uow
{
    public interface ISupportsRollback
    {
        void Rollback();

        Task RollbackAsync(CancellationToken cancellationToken);
    }
}
