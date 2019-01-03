﻿using System;

namespace ScrumPm.Domain.Common
{
    public interface IDomainEvent
    {
        int EventVersion { get; set; }

        DateTime OccurredOn { get; set; }
    }
}