using JetBrains.Annotations;

namespace ScrumPm.Domain.Common.Uow
{
    public interface IUnitOfWorkAccessor
    {
        [CanBeNull]
        IUnitOfWork UnitOfWork { get;  }

        void SetUnitOfWork([CanBeNull] IUnitOfWork unitOfWork);
    }
}
