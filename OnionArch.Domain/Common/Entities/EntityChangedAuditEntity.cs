using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Domain.Common.Entities
{
    public record EntityChangedAuditEntity(string EntityType, Guid EntityId, string ChangeType, string OriginalValues, string CurrentValues, string OccurredBy, DateTime OccurredOn) : BaseEntity
    {

    }
}
