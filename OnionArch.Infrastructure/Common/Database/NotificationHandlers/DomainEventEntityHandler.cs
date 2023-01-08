using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArch.Domain.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Infrastructure.Common.Database.NotificationHandlers
{
    public class DomainEventEntityHandler<TDbContext,TEntity> : INotificationHandler<DomainEventEntity<TEntity>> where TDbContext : DbContext where TEntity : BaseEntity
    {
        private readonly TDbContext _dbContext;

        public DomainEventEntityHandler(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(DomainEventEntity<TEntity> notification, CancellationToken cancellationToken)
        {
            switch(notification.EventName)
            {
                case "Create":
                    await _dbContext.Set<TEntity>().AddAsync(notification.AggregateRootEntity);
                    break;
            }

        }

    }
}
