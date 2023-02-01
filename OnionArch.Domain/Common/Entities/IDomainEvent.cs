using OnionArch.Domain.Common.DomainEvents;

namespace OnionArch.Domain.Common.Entities
{
    public interface IDomainEvent
    {
        IReadOnlyCollection<BaseDomainEvent> DomainEvents { get; }

        void AddDomainEvent(BaseDomainEvent domainEvent);
        void ClearDomainEvents();
    }
}