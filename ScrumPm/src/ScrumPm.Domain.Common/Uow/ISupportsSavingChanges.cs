using System.Threading;
using System.Threading.Tasks;

namespace ScrumPm.Domain.Common.Uow
{
    public interface ISupportsSavingChanges
    {
        void SaveChanges();

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
