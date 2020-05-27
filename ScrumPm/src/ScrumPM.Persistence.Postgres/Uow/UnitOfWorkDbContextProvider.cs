using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ScrumPm.Domain.Common.Extensions;
using ScrumPm.Domain.Common.Persistence;
using ScrumPm.Domain.Common.Uow;
using ScrumPM.Persistence.Postgres.Extensions;

namespace ScrumPM.Persistence.Postgres.Uow
{
   public class UnitOfWorkDbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
        where TDbContext : IEfCoreDbContext
    {
        private readonly IUnitOfWorkFactory<IUnitOfWork> _unitOfWorkFactory;
        private readonly IConnectionStringResolver _connectionStringResolver;
        private readonly TDbContext _dbContext;

        public UnitOfWorkDbContextProvider(
            IUnitOfWorkFactory<IUnitOfWork> unitOfWorkFactory,
            IConnectionStringResolver connectionStringResolver, TDbContext dbContext)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _connectionStringResolver = connectionStringResolver;
            _dbContext = dbContext;
        }

        public TDbContext GetDbContext()
        {
            var unitOfWork = _unitOfWorkFactory.Create();
            if (unitOfWork == null)
            {
                throw new Exception("A DbContext can only be created inside a unit of work!");
            }

            var dbContextKey = $"{typeof(TDbContext).FullName}";
            var databaseApi = unitOfWork.GetOrAddDatabaseApi(
                dbContextKey,
                () => new EfCoreDatabaseApi<TDbContext>(CreateDbContext(unitOfWork)
                ));

            return ((EfCoreDatabaseApi<TDbContext>)databaseApi).DbContext;
        }

        private TDbContext CreateDbContext(IUnitOfWork unitOfWork, string connectionStringName, string connectionString)
        {
                var dbContext = CreateDbContext(unitOfWork);

                if (unitOfWork.Options.Timeout.HasValue &&
               
                    !dbContext.Database.GetCommandTimeout().HasValue)
                {
                    dbContext.Database.SetCommandTimeout(unitOfWork.Options.Timeout.Value.TotalSeconds.To<int>());
                }

                dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
                dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                return dbContext;
        }

        private TDbContext CreateDbContext(IUnitOfWork unitOfWork)
        {
            return unitOfWork.Options.IsTransactional
                ? CreateDbContextWithTransaction(unitOfWork)
                : _dbContext;
        }

        public TDbContext CreateDbContextWithTransaction(IUnitOfWork unitOfWork) 
        {
            var transactionApiKey = $"EntityFrameworkCore_{typeof(TDbContext).FullName}";
            var activeTransaction = unitOfWork.FindTransactionApi(transactionApiKey) as EfCoreTransactionApi;

            if (activeTransaction == null)
            {
                var dbContextTransaction = unitOfWork.Options.IsolationLevel.HasValue
                    ? _dbContext.Database.BeginTransaction(unitOfWork.Options.IsolationLevel.Value)
                    : _dbContext.Database.BeginTransaction();

                unitOfWork.AddTransactionApi(
                    transactionApiKey,
                    new EfCoreTransactionApi(
                        dbContextTransaction,
                        _dbContext
                    )
                );

                return _dbContext;
            }

            if (_dbContext.As<DbContext>().HasRelationalTransactionManager())
            {
                _dbContext.Database.UseTransaction(activeTransaction.DbContextTransaction.GetDbTransaction());
            }
            else
            {
                _dbContext.Database.BeginTransaction(); 
            }

            activeTransaction.AttendedDbContexts.Add(_dbContext);
            return _dbContext;
        }
    }
}
