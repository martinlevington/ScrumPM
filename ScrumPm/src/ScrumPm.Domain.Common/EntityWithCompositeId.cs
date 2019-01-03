using System.Collections.Generic;
using System.Linq;

namespace ScrumPm.Domain.Common
{
    public abstract class EntityWithCompositeId : Entity
    {
      

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            return obj is EntityWithCompositeId other &&
                   GetIdentityComponents().SequenceEqual(other.GetIdentityComponents());
        }

        public override int GetHashCode()
        {
            return HashCodeHelper.CombineHashCodes(GetIdentityComponents());
        }
    }
}