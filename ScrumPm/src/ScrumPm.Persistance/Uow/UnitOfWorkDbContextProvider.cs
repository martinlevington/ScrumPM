using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ScrumPm.Domain.Common.Extensions;
using ScrumPm.Domain.Common.Persistence;
using ScrumPm.Domain.Common.Uow;
using ScrumPm.Persistence.EntityFrameworkCore;
using ScrumPm.Persistence.Extensions;

namespace ScrumPm.Persistence.Uow
{
   public class UnitOfWorkDbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
        where TDbContext : IEfCoreDbContext
    {
        private readonly IUnitOfWorkManager<IUnitOfWork> _unitOfWorkManager;
        private readonly IConnectionStringResolver _connectionStringResolver;
        private readonly TDbContext _dbContext;

        public UnitOfWorkDbContextProvider(
            IUnitOfWorkManager<IUnitOfWork> unitOfWorkManager,
            IConnectionStringResolver connectionStringResolver, TDbContext dbContext)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _connectionStringResolver = connectionStringResolver;
            _dbContext = dbContext;
        }

        public TDbContext GetDbContext()
        {
            var unitOfWork = _unitOfWorkManager.Create();
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
                    dbContext.Database.IsRelational() &&
                    !dbContext.Database.GetCommandTimeout().HasValue)
                {
                    dbContext.Database.SetCommandTimeout(unitOfWork.Options.Timeout.Value.TotalSeconds.To<int>());
                }

                //todo the following may come from options
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
                _dbContext.Database.BeginTransaction(); //TODO: Why not using the new created transaction?
            }

            activeTransaction.AttendedDbContexts.Add(_dbContext);

            return _dbContext;
        }
    }
}
