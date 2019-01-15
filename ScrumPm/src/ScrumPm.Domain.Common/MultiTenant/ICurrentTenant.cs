using System;
using JetBrains.Annotations;

namespace ScrumPm.Domain.Common.MultiTenant
{
    public interface ICurrentTenant
    {
        bool IsAvailable { get; }

        [CanBeNull]
        Guid? Id { get; }

        IDisposable Change(Guid? id);
    }
}
