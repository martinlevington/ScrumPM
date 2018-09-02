using System;
using System.Collections.Generic;
using System.Text;
using ScrumPm.Domain.Common;
using ScrumPm.Domain.Common.Persistence;
using ScrumPm.Persistence.Database;
using ScrumPm.Persistence.Database.UnitOfWork;

namespace ScrumPm.Persistence
{
    public abstract class Repository <DomainType, IdType, PersistenceType>  where DomainType : IAggregateRoot
    {

        protected readonly IUnitOfWork<ScrumPMContext> UnitOfWork;


        protected Repository(IUnitOfWork<ScrumPMContext> unitOfWork)
        {
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        }

      
    }
}
