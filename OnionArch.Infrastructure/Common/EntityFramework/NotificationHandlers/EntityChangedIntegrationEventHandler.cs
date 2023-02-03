using Dapr.Client;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OnionArch.Domain.Common.DomainEvents;
using OnionArch.Domain.Common.Entities;
using OnionArch.Domain.Common.IntegrationEvents;
using OnionArch.Domain.Common.Repositories;
using OnionArch.Domain.ProductInventory;
using OnionArch.Infrastructure.Common.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Google.Rpc.Context.AttributeContext.Types;

namespace OnionArch.Infrastructure.Common.EntityFramework.NotificationHandlers
{
    public class EntityChangedIntegrationEventHandler : INotificationHandler<EntityChangedDomainEvent>
    {
        private readonly RepositoryService<EntityChangedIntegrationEventEntity> _repositoryService;
        private readonly IConfiguration _configuration;
        private readonly DaprClient _daprClient;

        public EntityChangedIntegrationEventHandler(RepositoryService<EntityChangedIntegrationEventEntity> repositoryService, IConfiguration configuration, DaprClient daprClient)
        {
            _repositoryService = repositoryService;
            _configuration = configuration;
            _daprClient = daprClient;
        }

        public async Task Handle(EntityChangedDomainEvent notification, CancellationToken cancellationToken)
        {
            var entityFullName = notification.EventSource.GetType().FullName;
            //那实体变更要触发集成事件
            var integrationEventConfig = _configuration.GetRequiredSection("EntityChangedIntegrationEventConfig");
            var pubsubName = integrationEventConfig["PubsubName"];
            var topics = integrationEventConfig.GetSection("Topics").GetChildren();
            EntityChangedIntegrationEventEntity entity = new EntityChangedIntegrationEventEntity(pubsubName, entityFullName, notification.EventSource.Id, notification.ChangeType, notification.OccurredBy, notification.OccurredOn);
            EntityChangedIntegrationEventData eventData = new EntityChangedIntegrationEventData(entity.TenantId,
               entity.EntityType, entity.EntityId, entity.ChangeType,
               entity.OccurredBy, entity.OccurredOn);
            foreach (var topic in topics)
            {
                //要发布集成事件的实体和数据变更类型
                if (topic["EntityFullName"] == entityFullName && topic["ChangeType"] == notification.ChangeType)
                {
                    entity.TopicName = topic["TopicName"];
                    switch (notification.ChangeType)
                    {
                        case "Added":
                            entity.CurrentValues = JsonConvert.SerializeObject(notification.CurrentValues, Formatting.Indented);
                            entity.OriginalValues = "";
                            eventData.CurrentValues = notification.CurrentValues;
                            break;
                        case "Modified":
                            //那些属性改变要发布集成事件
                            var propertiesConfig = topic["Properties"] + ";";
                            foreach (var originalValue in notification.OriginalValues)
                            {
                                //属性值改变
                                if (propertiesConfig.Contains(originalValue.Key + ";") && notification.CurrentValues.TryGetValue(originalValue.Key, out object? value))
                                {
                                    if (value != originalValue.Value)
                                    {
                                        entity.CurrentValues = JsonConvert.SerializeObject(notification.CurrentValues, Formatting.Indented);
                                        entity.OriginalValues = JsonConvert.SerializeObject(notification.OriginalValues, Formatting.Indented);
                                        eventData.CurrentValues = notification.CurrentValues;
                                        eventData.OriginalValues = notification.OriginalValues;
                                    }
                                }
                            }
                            break;
                        case "Deleted":
                            entity.CurrentValues = "";
                            entity.OriginalValues = JsonConvert.SerializeObject(notification.OriginalValues, Formatting.Indented);
                            eventData.OriginalValues = notification.OriginalValues;
                            break;
                        default:
                            break;
                    }
                    entity.Id = Guid.NewGuid();
                    entity.TenantId = notification.EventSource.TenantId;
                    await _repositoryService.Add(entity);
                    //发布集成事件
                    //await _daprClient.PublishEventAsync<EntityChangedIntegrationEventData>(pubsubName, entity.TopicName, eventData);
                }
            }

        }

    }
}
