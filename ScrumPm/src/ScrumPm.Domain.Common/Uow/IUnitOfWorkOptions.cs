using System;
using System.Data;

namespace ScrumPm.Domain.Common.Uow
{
    public interface IUnitOfWorkOptions
    {
        bool IsTransactional { get; }

        IsolationLevel? IsolationLevel { get; }

        TimeSpan? Timeout { get; }
    }
}
