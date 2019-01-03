using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ScrumPm.Domain.Common
{


    public abstract class Entity : IEntity, IEquatable<Entity>
    {
      


        protected Entity()
        {

        }

       

        public bool Equals(Entity other)
        {
            if (!(other is Entity))
            {
                return false;
            }

            //Same instances must be considered as equal
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            //Must have a IS-A relation of types or must be same type
            var typeOfThis = GetType().GetTypeInfo();
            var typeOfOther = other.GetType().GetTypeInfo();
            if (!typeOfThis.IsAssignableFrom(typeOfOther) && !typeOfOther.IsAssignableFrom(typeOfThis))
            {
                return false;
            }


            return GetIdentityComponents().SequenceEqual(other.GetIdentityComponents());
        }


        /// <summary>
        /// When overriden in a derived class, gets all components of the identity of the entity.
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<object> GetIdentityComponents();

    }
}