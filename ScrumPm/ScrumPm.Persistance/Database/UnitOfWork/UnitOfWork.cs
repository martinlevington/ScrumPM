using System;
using Microsoft.EntityFrameworkCore;
using ScrumPm.Common;
using ScrumPm.Common.Persistence;

namespace ScrumPm.Persistence.Database.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IContextFactory<ScrumPMContext> _contextFactory;

        public UnitOfWork(IContextFactory<ScrumPMContext> contextFactory)
        {
            if (contextFactory == null)
            {
                throw new ArgumentNullException("Context factory");
            }

            _contextFactory = contextFactory;
        }

        public DbContext GetContext()
        {
            return _contextFactory.Create();
        }

        public void Commit()
        {
            _contextFactory.Create().SaveChanges();
        }

        public async void CommitAsync()
        {
            await _contextFactory.Create().SaveChangesAsync();
        }

        public void RegisterUpdate(IAggregateRoot aggregateRoot)
        {
            // todo check if the item exists ??
            _contextFactory.Create().Update(aggregateRoot);
        }

        public void RegisterInsertion(IAggregateRoot aggregateRoot)
        {
            // todo check if the item exists ??
            _contextFactory.Create().Add(aggregateRoot);
        }

        public void RegisterDeletion(IAggregateRoot aggregateRoot)
        {
            // todo check if the item exists ??
            _contextFactory.Create().Remove(aggregateRoot);
        }
    }
}