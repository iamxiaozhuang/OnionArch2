using OnionArch.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Domain.Common.IntegrationEvents
{
    public class EntityChangedIntegrationEventData
    {
        public EntityChangedIntegrationEventData(Guid tenantId, string entityType, Guid entityId, string changeType, string occurredBy, DateTime occurredOn)
        {
            TenantId = tenantId;
            EntityType = entityType;
            EntityId = entityId;
            ChangeType = changeType;
            OccurredBy = occurredBy;
            OccurredOn = occurredOn;
        }

        public string ChangeType { get; set; }
        public string OccurredBy { get; }
        public DateTime OccurredOn { get; }
        public Guid TenantId { get; }
        public string EntityType { get; }
        public Guid EntityId { get; }
        public Dictionary<string, object?> OriginalValues { get; set; }
        public Dictionary<string, object?> CurrentValues { get; set; }
    }
}
