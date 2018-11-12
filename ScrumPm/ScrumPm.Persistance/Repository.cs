using System;
using ScrumPm.Domain.Common;
using ScrumPm.Domain.Common.Persistence;
using ScrumPm.Persistence.Database;

namespace ScrumPm.Persistence
{
    public abstract class Repository <TDomainType, TIdType, TPersistenceType>  where TDomainType : IAggregateRoot
    {

        protected readonly IUnitOfWork<ScrumPmContext> UnitOfWork;


        protected Repository(IUnitOfWork<ScrumPmContext> unitOfWork)
        {
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        }

      
    }
}
