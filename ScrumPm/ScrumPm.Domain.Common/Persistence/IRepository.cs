using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using ScrumPm.Domain.Common.DependancyInjection;
using ScrumPm.Domain.Common.PagedList;
using ScrumPm.Domain.Common.Uow;

namespace ScrumPm.Domain.Common.Persistence
{
    public interface IRepository : IUnitOfWorkEnabled, ITransientDependency
    {
      

    }

    public interface IRepository<TEntity> : IReadOnlyRepository<TEntity>, IBasicRepository<TEntity>
        where TEntity : class, IDataEntity
    {


        void Delete([NotNull] Expression<Func<TEntity, bool>> predicate, bool autoSave = false);

        Task DeleteAsync([NotNull] Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default);

    }

    public interface IRepository<TEntity, TKey> : IRepository<TEntity>, IReadOnlyRepository<TEntity, TKey>, IBasicRepository<TEntity, TKey>
        where TEntity : class, IDataEntity<TKey>
    {
    }

}