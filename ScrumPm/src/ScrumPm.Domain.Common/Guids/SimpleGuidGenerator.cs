using System;

namespace ScrumPm.Domain.Common.Guids
{
    public class SimpleGuidGenerator : IGuidGenerator
    {
        public static SimpleGuidGenerator Instance { get; } = new SimpleGuidGenerator();

        public virtual Guid Create()
        {
            return Guid.NewGuid();
        }
    }
}
