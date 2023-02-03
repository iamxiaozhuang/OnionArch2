using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
        private readonly IConfiguration _configuration;

        public EntityChangedAuditHandler(RepositoryService<EntityChangedAuditEntity> repositoryService, IConfiguration configuration)
        {
            _repositoryService = repositoryService;
            _configuration = configuration;
        }

        public async Task Handle(EntityChangedDomainEvent notification, CancellationToken cancellationToken)
        {
            var entityFullName = notification.EventSource.GetType().FullName;
            //那些实体要审计
            var auditLogsConfigs = _configuration.GetRequiredSection("EntityChangedAuditLogsConfig").GetChildren();
            EntityChangedAuditEntity entityChangedAuditEntity = new EntityChangedAuditEntity(entityFullName, notification.EventSource.Id, notification.ChangeType, notification.OccurredBy, notification.OccurredOn);
            foreach (var entityConfig in auditLogsConfigs)
            {
                //要审计的实体和数据变更类型
                if (entityConfig["EntityFullName"] == entityFullName && entityConfig["ChangeType"] == notification.ChangeType)
                {
                    switch (notification.ChangeType)
                    {
                        case "Added":
                            entityChangedAuditEntity.CurrentValues = JsonConvert.SerializeObject(notification.CurrentValues, Formatting.Indented);
                            entityChangedAuditEntity.OriginalValues = "";
                            break;
                        case "Modified":
                            //那些属性要审计
                            var propertiesConfig = entityConfig["Properties"] + ";";
                            foreach (var originalValue in notification.OriginalValues)
                            {
                                //要审计的属性值改变
                                if (propertiesConfig.Contains(originalValue.Key + ";") && notification.CurrentValues.TryGetValue(originalValue.Key, out object? value))
                                {
                                    if (value != originalValue.Value)
                                    {
                                        entityChangedAuditEntity.CurrentValues = JsonConvert.SerializeObject(notification.CurrentValues, Formatting.Indented);
                                        entityChangedAuditEntity.OriginalValues = JsonConvert.SerializeObject(notification.OriginalValues, Formatting.Indented);
                                    }
                                }
                            }
                            break;
                        case "Deleted":
                            entityChangedAuditEntity.CurrentValues = "";
                            entityChangedAuditEntity.OriginalValues = JsonConvert.SerializeObject(notification.OriginalValues, Formatting.Indented);
                            break;
                        default:
                            break;
                    }
                }
            }
            entityChangedAuditEntity.Id = Guid.NewGuid();
            entityChangedAuditEntity.TenantId = notification.EventSource.TenantId;
            await _repositoryService.Add(entityChangedAuditEntity);
        }

    }
}
