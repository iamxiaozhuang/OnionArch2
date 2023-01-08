using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using MediatR;
using Mapster;
using System.Net.NetworkInformation;
using OnionArch.Domain.Common.Database;
using Newtonsoft.Json;

namespace OnionArch.Domain.Common.Base
{
    public record AggregateRoot<TEntity> : BaseEntity where TEntity : BaseEntity
    {
        [NotMapped]
        private List<DomainEventEntity<TEntity>> _domainEvents;
       
        public void Create()
        {
            AddDomainEvent(new DomainEventEntity<TEntity>("Create", Id, JsonConvert.SerializeObject(this as TEntity)));
        }

        public void Update(string updateData)
        {
            AddDomainEvent(new DomainEventEntity<TEntity>("Update", Id, JsonConvert.SerializeObject(this as TEntity)));
        }

        public void Delete()
        {
            AddDomainEvent(new DomainEventEntity<TEntity>("Delete", Id, JsonConvert.SerializeObject(this as TEntity)));
        }


        [Required]
        public ICollection<DomainEventEntity<TEntity>> DomainEvents => _domainEvents;

        /// <summary>
        /// Add domain event.
        /// </summary>
        /// <param name="domainEvent"></param>
        private void AddDomainEvent(DomainEventEntity<TEntity> domainEvent)
        {
            _domainEvents = _domainEvents ?? new List<DomainEventEntity<TEntity>>();
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
