namespace ScrumPm.Common
{
    using System;

    public interface IDomainEvent
    {
        int EventVersion { get; set;  }

        DateTime OccurredOn { get; set; }
    }
}
