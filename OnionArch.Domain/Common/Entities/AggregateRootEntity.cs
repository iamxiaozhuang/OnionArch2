using OnionArch.Domain.Common.DomainEvents;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnionArch.Domain.Common.Entities
{
    public record AggregateRootEntity : BaseEntity, IDomainEvent
    {
        [NotMapped]
        private List<BaseDomainEvent> _domainEvents;

        /// <summary>
        /// Domain events occurred.
        /// </summary>
        [NotMapped]
        public IReadOnlyCollection<BaseDomainEvent> DomainEvents => _domainEvents?.AsReadOnly();
        /// <summary>
        /// Add domain event.
        /// </summary>
        /// <param name="domainEvent"></param>
        public void AddDomainEvent(BaseDomainEvent domainEvent)
        {
            _domainEvents = _domainEvents ?? new List<BaseDomainEvent>();
            _domainEvents.Add(domainEvent);
        }

        /// <summary>
        /// Clear domain events.
        /// </summary>
        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

    }
}
