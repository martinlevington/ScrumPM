namespace ScrumPm.Domain.Common
{

    using System;

    public interface IDomainEventSubscriber<T>
        where T : IDomainEvent
    {
        void HandleEvent(T domainEvent);

        Type SubscribedToEventType();
    }

}