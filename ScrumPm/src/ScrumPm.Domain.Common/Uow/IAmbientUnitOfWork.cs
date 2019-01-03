using JetBrains.Annotations;

namespace ScrumPm.Domain.Common.Uow
{
    public interface IAmbientUnitOfWork 
    {
        IUnitOfWork UnitOfWork { get;  }

        void SetUnitOfWork([CanBeNull] IUnitOfWork unitOfWork);
    }
}
