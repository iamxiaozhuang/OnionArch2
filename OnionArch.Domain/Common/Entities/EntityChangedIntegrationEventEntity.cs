using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Domain.Common.Entities
{
    public record EntityChangedIntegrationEventEntity(string PubsubName, string EntityType, Guid EntityId, string ChangeType, string OccurredBy, DateTime OccurredOn) : BaseEntity
    {
        public string TopicName { get; set; }
        public string OriginalValues { get; set; }
        public string CurrentValues { get; set; }
    }
}
