using System.Threading;
using System.Threading.Tasks;
using ScrumPm.Domain.Common.Uow;

namespace ScrumPM.Persistence.Postgres.Uow
{
    public class EfCoreDatabaseApi<TDbContext> : IDatabaseApi, ISupportsSavingChanges
        where TDbContext : IEfCoreDbContext
    {
        public EfCoreDatabaseApi(TDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public TDbContext DbContext { get; }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return DbContext.SaveChangesAsync(cancellationToken);
        }

        public void SaveChanges()
        {
            DbContext.SaveChanges();
        }
    }
}