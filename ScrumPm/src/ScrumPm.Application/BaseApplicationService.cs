using ScrumPm.Domain.Common.Uow;

namespace ScrumPm.Application
{
    public abstract class BaseApplicationService
    {
        public IUnitOfWorkFactory<IUnitOfWork> UnitOfWorkFactory { get; set; }

        protected IUnitOfWork CurrentUnitOfWork { get; set; }
       
       protected  BaseApplicationService(IUnitOfWorkFactory<IUnitOfWork> unitOfWorkFactory)
        {
            UnitOfWorkFactory = unitOfWorkFactory;
           
        }


    }
}
