using MediatR;
using OnionArch.Domain.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Domain.Common.Base
{
    public record DomainEventEntity<TEntity>(string EventName, Guid AggregateRootId, string EntityJson) : BaseEntity, INotification where TEntity : BaseEntity
    {
        public virtual TEntity AggregateRootEntity { get; set; }
        public string OccurredBy { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}
