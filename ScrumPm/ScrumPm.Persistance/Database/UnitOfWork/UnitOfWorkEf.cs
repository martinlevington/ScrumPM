using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ScrumPm.Domain.Common.Persistence;

namespace ScrumPm.Persistence.Database.UnitOfWork
{
    public class UnitOfWorkEf<T> : IUnitOfWork<T> where T : DbContext
    {
        private readonly IContextFactory<T> _contextFactory;
        private T _dataContext;

        public UnitOfWorkEf(IContextFactory<T> databaseFactory)
        {
            this._contextFactory = databaseFactory;
        }

        private T DataContext
        {
            get { return _dataContext ?? (_dataContext = _contextFactory.Create()); }
        }

        public T GetContext()
        {
            return DataContext;
        }

        public void Start()
        {
            DataContext.Database.BeginTransaction();
        }

        public async Task StartAsync()
        {
           await DataContext.Database.BeginTransactionAsync();
        }

        public void CommitAsync()
        {
           DataContext.Database.CommitTransaction();
        }

        public void Commit()
        {  DataContext.Database.CommitTransaction();
          
        }


        public void SaveChanges()
        {
            DataContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await DataContext.SaveChangesAsync();
        }
        

    }
}
