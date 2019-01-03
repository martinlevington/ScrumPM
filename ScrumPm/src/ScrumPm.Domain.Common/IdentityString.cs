using System;

namespace ScrumPm.Domain.Common
{
    public abstract class IdentityString : IEquatable<IdentityString>, IIdentity<string>
    {
        public IdentityString()
        {
            Id = Guid.NewGuid().ToString();
        }

        public IdentityString(string id)
        {
            Id = id;
        }

        public string Id { get; }

        public bool Equals(IdentityString id)
        {
            if (ReferenceEquals(this, id)) return true;

            if (ReferenceEquals(null, id)) return false;

            return Id.Equals(id.Id);
        }

        public override bool Equals(object anotherObject)
        {
            return Equals(anotherObject as IdentityString);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        public override string ToString()
        {
            return GetType().Name + " [Id=" + Id + "]";
        }
    }
}