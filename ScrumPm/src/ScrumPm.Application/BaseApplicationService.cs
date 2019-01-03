using ScrumPm.Domain.Common.Uow;

namespace ScrumPm.Application
{
    public abstract class BaseApplicationService
    {
        public IUnitOfWorkManager<IUnitOfWork> UnitOfWorkManager { get; set; }

        protected IUnitOfWork CurrentUnitOfWork { get; set; }
       
       protected  BaseApplicationService(IUnitOfWorkManager<IUnitOfWork> unitOfWorkManager)
        {
            UnitOfWorkManager = unitOfWorkManager;
           
        }


    }
}
