using JetBrains.Annotations;

namespace ScrumPm.Domain.Common.Uow
{
    public interface IUnitOfWorkManager<out T>
    {


        [NotNull]
        T Create([NotNull] string name,[NotNull] UnitOfWorkOptions options);

        [NotNull]
        T Create([NotNull] string name);

        [NotNull]
        T Create();

      
    }
}
