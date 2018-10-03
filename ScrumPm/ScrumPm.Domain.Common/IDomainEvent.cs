namespace ScrumPm.Domain.Common
{


    using System;

    public interface IDomainEvent
    {
        int EventVersion { get; set; }

        DateTime OccurredOn { get; set; }
    }
}