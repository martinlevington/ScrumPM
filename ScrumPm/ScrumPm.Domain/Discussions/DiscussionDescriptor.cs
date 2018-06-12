namespace ScrumPm.Domain.Discussions
{
    using ScrumPm.Common;

    public class DiscussionDescriptor : ValueObject
    {
        public const string UNDEFINED_ID = "UNDEFINED";

        public DiscussionDescriptor(string id)
        {
            this.Id = id;
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
                return this.Id.Equals(UNDEFINED_ID);
            }
        }

        public override string ToString()
        {
            return "DiscussionDescriptor [id=" + Id + "]";
        }

        protected override System.Collections.Generic.IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Id;
        }
    }
}
