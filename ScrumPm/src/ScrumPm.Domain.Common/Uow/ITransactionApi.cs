using System;
using System.Threading.Tasks;

namespace ScrumPm.Domain.Common.Uow
{
    public interface ITransactionApi : IDisposable
    {
        void Commit();

        Task CommitAsync();
    }
}
