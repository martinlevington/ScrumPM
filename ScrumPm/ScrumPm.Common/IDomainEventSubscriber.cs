

using System;

namespace ScrumPm.Domain.Common
{
    public interface IDomainEventSubscriber<T>
        where T : IDomainEvent
    {
        void HandleEvent(T domainEvent);

        Type SubscribedToEventType();
    }
}
