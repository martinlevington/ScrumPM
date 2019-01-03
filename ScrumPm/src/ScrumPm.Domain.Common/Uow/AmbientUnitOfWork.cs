using System.Threading;
using ScrumPm.Domain.Common.DependencyInjection;

namespace ScrumPm.Domain.Common.Uow
{
    /// <summary>
    /// Represents ambient UoW that is local to a given asynchronous control flow.
    /// </summary>
    [ServiceRegistrationLocator(typeof(IAmbientUnitOfWork), typeof(AmbientUnitOfWork))]
    public class AmbientUnitOfWork : IAmbientUnitOfWork, ISingletonDependency
    {
        public IUnitOfWork UnitOfWork => _currentUow.Value;

        private readonly AsyncLocal<IUnitOfWork> _currentUow;

        public AmbientUnitOfWork()
        {
            _currentUow = new AsyncLocal<IUnitOfWork>();
        }

        public void SetUnitOfWork(IUnitOfWork unitOfWork)
        {
            _currentUow.Value = unitOfWork;
        }
    }
}
