using ScrumPm.Persistence.Database.UnitOfWork;

namespace ScrumPm.Common.Persistence
{
    public interface IUnitOfWork
    {
        void RegisterUpdate(IAggregateRoot aggregateRoot);
        void RegisterInsertion(IAggregateRoot aggregateRoot);
        void RegisterDeletion(IAggregateRoot aggregateRoot);

        void CommitAsync();
        void Commit();
    }
}
