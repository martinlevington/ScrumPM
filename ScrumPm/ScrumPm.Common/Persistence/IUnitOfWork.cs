using System.Threading.Tasks;

namespace ScrumPm.Domain.Common.Persistence
{
    public interface IUnitOfWork 
    {
      
        void Start();
        Task StartAsync();
        void CommitAsync();
        void Commit();
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
