using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArch.Domain.Common.Base;
using OnionArch.Domain.Common.Database;
using OnionArch.Domain.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Infrastructure.Common.Database.RequestHandlers
{
    public class ReadEntityRequestHandler<TDbContext,TEntity> : IRequestHandler<ReadEntityRequest<TEntity>,TEntity> where TDbContext : DbContext where TEntity : BaseEntity
    {
        private readonly TDbContext _dbContext;

        public ReadEntityRequestHandler(TDbContext dbContext)
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

        public async Task<TEntity> Handle(ReadEntityRequest<TEntity> request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(p => p.Id == request.Id);
            if (entity == null)
            {
                throw new NotFoundException(nameof(TEntity), request.Id, "The resource is not found");
            }
            return entity;
        }

    }
}
