using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ScrumPm.Domain.Common.Extensions;
using ScrumPm.Domain.Common.Uow;
using ScrumPM.Persistence.Postgres.Extensions;

namespace ScrumPM.Persistence.Postgres
{
    public class EfCoreTransactionApi : ITransactionApi, ISupportsRollback
    {
        public EfCoreTransactionApi(IDbContextTransaction dbContextTransaction, IEfCoreDbContext starterDbContext)
        {
            DbContextTransaction = dbContextTransaction;
            StarterDbContext = starterDbContext;
            AttendedDbContexts = new List<IEfCoreDbContext>();
        }

        public IDbContextTransaction DbContextTransaction { get; }
        public IEfCoreDbContext StarterDbContext { get; }
        public List<IEfCoreDbContext> AttendedDbContexts { get; }

        public void Rollback()
        {
            DbContextTransaction.Rollback();
        }

        public Task RollbackAsync(CancellationToken cancellationToken)
        {
            DbContextTransaction.Rollback();
            return Task.CompletedTask;
        }

        public void Commit()
        {
            DbContextTransaction.Commit();

            foreach (var dbContext in AttendedDbContexts)
            {
                if (dbContext.As<DbContext>().HasRelationalTransactionManager())
                {
                    continue; //Relational databases use the shared transaction
                }

                dbContext.Database.CommitTransaction();
            }
        }
        public Task CommitAsync()
        {
            Commit();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            DbContextTransaction.Dispose();
        }
    }
}