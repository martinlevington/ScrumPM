﻿namespace ScrumPm.Common
{
    using System.Collections.Generic;
    using System.Linq;

    public abstract class EntityWithCompositeId : Entity
    {
        /// <summary>
        /// When overriden in a derived class, gets all components of the identity of the entity.
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable<object> GetIdentityComponents();

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj)) return true;

            if (object.ReferenceEquals(null, obj)) return false;

            if (GetType() != obj.GetType()) return false;

            return obj is EntityWithCompositeId other && GetIdentityComponents().SequenceEqual(other.GetIdentityComponents());
        }

        public override int GetHashCode()
        {
            return HashCodeHelper.CombineHashCodes(GetIdentityComponents());
        }
    }
}
