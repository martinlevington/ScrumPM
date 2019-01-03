using JetBrains.Annotations;

namespace ScrumPm.Domain.Common.Uow
{
    public interface IUnitOfWorkFactory<out T>
    {


        [NotNull]
        T Create([NotNull] string name,[NotNull] UnitOfWorkOptions options);

        [NotNull]
        T Create([NotNull] string name);

        [NotNull]
        T Create();

      
    }
}
