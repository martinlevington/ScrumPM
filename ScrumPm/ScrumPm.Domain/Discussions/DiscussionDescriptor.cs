using System.Collections.Generic;
using ScrumPm.Domain.Common;

namespace ScrumPm.Domain.Discussions
{


    public class DiscussionDescriptor : ValueObject
    {
        public const string UndefinedId = "UNDEFINED";

        public DiscussionDescriptor(string id)
        {
            Id = id;
        }

        public DiscussionDescriptor(DiscussionDescriptor discussionDescriptor)
            : this(discussionDescriptor.Id)
        {
        }

        public string Id { get; private set; }

        public bool IsUndefined
        {
            get
            {
                return Id.Equals(UndefinedId);
            }
        }

        public override string ToString()
        {
            return "DiscussionDescriptor [id=" + Id + "]";
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
        }
    }
}
