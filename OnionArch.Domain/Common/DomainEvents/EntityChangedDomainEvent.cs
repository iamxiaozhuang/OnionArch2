using OnionArch.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Domain.Common.DomainEvents
{
    public class EntityChangedDomainEvent : BaseDomainEvent
    {
        public EntityChangedDomainEvent(BaseEntity entity, string occurredBy,string originalValues, string currentValues) : base(entity, nameof(EntityChangedDomainEvent), occurredBy)
        {
            OriginalValues = originalValues;
            CurrentValues = currentValues;
        }

        public string ChangeType { get; set; }
        public string OriginalValues { get; }
        public string CurrentValues { get; }
    }
}
