using System;

namespace ScrumPm.Domain.Common
{
    public abstract class Identity : IEquatable<Identity>, IIdentity<Guid>
    {
        public Identity()
        {
            Id = Guid.NewGuid();
        }

        public Identity(Guid id)
        {
            Id = id;
        }

        public Identity(string id)
        {
            Id = new Guid(id);
        }

        public Guid Id { get; }

        public Guid GetId()
        {
            return Id;
        }

        public bool Equals(Identity id)
        {
            if (ReferenceEquals(this, id))
            {
                return true;
            }

            if (ReferenceEquals(null, id))
            {
                return false;
            }

            return Id.Equals(id.Id);
        }

        public override bool Equals(object anotherObject)
        {
            return Equals(anotherObject as Identity);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        public string ToNameString()
        {
            return GetType().Name + " [Id=" + Id + "]";
        }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}