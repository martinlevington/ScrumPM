using System.Threading.Tasks;

namespace ScrumPm.Domain.Common.Persistence
{
    public interface IUnitOfWorkOld<T> where T : class
    {
        T GetContext();
        void Start();
        Task StartAsync();
        void CommitAsync();
        void Commit();
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
