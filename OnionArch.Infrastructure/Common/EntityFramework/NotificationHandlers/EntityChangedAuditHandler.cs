using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArch.Domain.Common.DomainEvents;
using OnionArch.Domain.Common.Entities;
using OnionArch.Domain.Common.Repositories;
using OnionArch.Domain.ProductInventory;
using OnionArch.Infrastructure.Common.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Infrastructure.Common.EntityFramework.NotificationHandlers
{
    public class EntityChangedAuditHandler : INotificationHandler<EntityChangedDomainEvent>
    {
        private readonly RepositoryService<EntityChangedAuditEntity> _repositoryService;

        public EntityChangedAuditHandler(RepositoryService<EntityChangedAuditEntity> repositoryService)
        {
            _repositoryService = repositoryService;
        }

        public async Task Handle(EntityChangedDomainEvent notification, CancellationToken cancellationToken)
        {
            var entity = new EntityChangedAuditEntity(notification.EventSource.GetType().FullName, notification.EventSource.Id, notification.ChangeType, notification.OriginalValues, notification.CurrentValues, notification.OccurredBy, notification.OccurredOn);
            entity.Id = Guid.NewGuid();
            entity.TenantId = notification.EventSource.TenantId;
            await _repositoryService.Create(entity);
        }

    }
}
