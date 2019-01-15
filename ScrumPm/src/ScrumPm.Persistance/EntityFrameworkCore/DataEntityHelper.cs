﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using ScrumPm.Domain.Common.Persistence;

namespace ScrumPm.Persistence.EntityFrameworkCore
{
    /// <summary>
    /// Some helper methods for entities.
    /// </summary>
    public static class DataEntityHelper
    {
        public static bool IsEntity([NotNull] Type type)
        {
            return typeof(IDataEntity).IsAssignableFrom(type);
        }

        public static bool HasDefaultId<TKey>(IDataEntity<TKey> entity)
        {
            if (EqualityComparer<TKey>.Default.Equals(entity.Id, default))
            {
                return true;
            }

            //Workaround for EF Core since it sets int/long to min value when attaching to dbcontext
            if (typeof(TKey) == typeof(int))
            {
                return Convert.ToInt32(entity.Id) <= 0;
            }

            if (typeof(TKey) == typeof(long))
            {
                return Convert.ToInt64(entity.Id) <= 0;
            }

            return false;
        }

        /// <summary>
        /// Tries to find the primary key type of the given entity type.
        /// May return null if given type does not implement <see cref="IEntity{TKey}"/>
        /// </summary>
        [CanBeNull]
        public static Type FindPrimaryKeyType<TEntity>()
            where TEntity : IDataEntity
        {
            return FindPrimaryKeyType(typeof(TEntity));
        }

        /// <summary>
        /// Tries to find the primary key type of the given entity type.
        /// May return null if given type does not implement <see cref="IEntity{TKey}"/>
        /// </summary>
        [CanBeNull]
        public static Type FindPrimaryKeyType([NotNull] Type entityType)
        {
            if (!typeof(IDataEntity).IsAssignableFrom(entityType))
            {
                throw new Exception($"Given {nameof(entityType)} is not an entity. It should implement {typeof(IDataEntity).AssemblyQualifiedName}!");
            }

            foreach (var interfaceType in entityType.GetTypeInfo().GetInterfaces())
            {
                if (interfaceType.GetTypeInfo().IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IDataEntity<>))
                {
                    return interfaceType.GenericTypeArguments[0];
                }
            }

            return null;
        }

        public static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId<TEntity, TKey>(TKey id)
            where TEntity : IDataEntity<TKey>
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));
            var lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, nameof(IDataEntity<TKey>.Id)),
                Expression.Constant(id, typeof(TKey))
            );

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }
    }
}
