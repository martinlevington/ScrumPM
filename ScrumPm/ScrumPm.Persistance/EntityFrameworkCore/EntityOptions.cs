using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using ScrumPm.Domain.Common;
using ScrumPm.Domain.Common.Extensions;
using ScrumPm.Domain.Common.Persistence;
using ScrumPm.Domain.Common.Validation;

namespace ScrumPm.Persistence.EntityFrameworkCore
{
    public class EntityOptions<TEntity>
        where TEntity : IDataEntity
    {
        public static EntityOptions<TEntity> Empty { get; } = new EntityOptions<TEntity>();

        public Func<IQueryable<TEntity>, IQueryable<TEntity>> DefaultWithDetailsFunc { get; set; }
    }

    public class EntityOptions
    {
        private readonly IDictionary<Type, object> _options;

        public EntityOptions()
        {
            _options = new Dictionary<Type, object>();
        }

        public EntityOptions<TEntity> GetOrNull<TEntity>()
            where TEntity : IDataEntity
        {
            return _options.GetOrDefault(typeof(TEntity)) as EntityOptions<TEntity>;
        }

        public void Entity<TEntity>([NotNull] Action<EntityOptions<TEntity>> optionsAction)
            where TEntity : IDataEntity
        {
            Check.NotNull(optionsAction, nameof(optionsAction));

            optionsAction(
                _options.GetOrAdd(
                    typeof(TEntity),
                    () => new EntityOptions<TEntity>()
                ) as EntityOptions<TEntity>
            );
        }
    }
}
