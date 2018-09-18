using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ScrumPm.Domain.Common.Persistence;

namespace ScrumPm.Persistence.Database.UnitOfWork
{
    public class UnitOfWorkEf : IUnitOfWork
    {
        private readonly IContextFactory<ScrumPMContext> _contextFactory;
        private ScrumPMContext _dataContext;

        public UnitOfWorkEf(IContextFactory<ScrumPMContext> databaseFactory)
        {
            _contextFactory = databaseFactory;
            _dataContext = databaseFactory.Create();
        }

    
        //private T DataContext
        //{
        //    get { return _dataContext ?? (_dataContext = _contextFactory.Create()); }
        //}

        //public T GetContext()
        //{
        //    return DataContext;
        //}

        public void Start()
        {
            _dataContext.Database.BeginTransaction();
        }

        public async Task StartAsync()
        {
           await _dataContext.Database.BeginTransactionAsync();
        }

        public void CommitAsync()
        {
            _dataContext.Database.CommitTransaction();
        }

        public void Commit()
        {  _dataContext.Database.CommitTransaction();
          
        }


        public void SaveChanges()
        {
            _dataContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _dataContext.SaveChangesAsync();
        }
        

    }
}
