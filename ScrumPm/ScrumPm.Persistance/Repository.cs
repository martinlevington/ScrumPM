using System;
using System.Collections.Generic;
using System.Text;
using ScrumPm.Common;
using ScrumPm.Common.Persistence;
using ScrumPm.Persistence.Database;
using ScrumPm.Persistence.Database.UnitOfWork;

namespace ScrumPm.Persistence
{
    public abstract class Repository <DomainType, IdType, PersistenceType> : IUnitOfWorkRepository where DomainType : IAggregateRoot
    {

        private readonly IUnitOfWork _unitOfWork;
      

        public Repository(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException("Unit of work");
            }

            _unitOfWork = unitOfWork;

        }

        public void Update(DomainType aggregate)
        {
            _unitOfWork.RegisterUpdate(aggregate);
        }

        public void PersistInsertion(IAggregateRoot aggregateRoot)
        {
            throw new NotImplementedException();
        }

        public void PersistUpdate(IAggregateRoot aggregateRoot)
        {
            throw new NotImplementedException();
        }

        public void PersistDeletion(IAggregateRoot aggregateRoot)
        {
            throw new NotImplementedException();
        }
    }
}
