using MediatR;
using OnionArch.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Domain.Common.DomainEvents
{
    public class BaseDomainEvent: INotification
    {

        public BaseDomainEvent(BaseEntity eventSource, string eventName, string occurredBy)
        {
            EventSource = eventSource;
            EventName = eventName;
            OccurredBy = occurredBy;
            OccurredOn = DateTime.UtcNow;
        }
        public BaseEntity EventSource { get; private set; }
        public string EventName { get; private set; }
        public string OccurredBy { get; private set; }
        public DateTime OccurredOn { get; init; }
    }
}
