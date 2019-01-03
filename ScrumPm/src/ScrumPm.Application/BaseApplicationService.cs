using ScrumPm.Domain.Common.Uow;

namespace ScrumPm.Application
{
    public abstract class BaseApplicationService
    {
        public IUnitOfWorkManager<IUnitOfWork> UnitOfWorkManager { get; set; }

        protected IUnitOfWork CurrentUnitOfWork => UnitOfWorkManager?.Current;


        protected  BaseApplicationService(IUnitOfWorkManager<IUnitOfWork> unitOfWorkManager)
        {
            UnitOfWorkManager = unitOfWorkManager;
        }


    }
}
