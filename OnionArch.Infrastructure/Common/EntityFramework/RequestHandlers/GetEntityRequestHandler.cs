using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArch.Domain.Common.Entities;
using OnionArch.Domain.Common.Exceptions;
using OnionArch.Domain.Common.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Infrastructure.Common.EntityFramework.RequestHandlers
{
    public class GetEntityRequestHandler<TDbContext,TEntity> : IRequestHandler<GetEntityRequest<TEntity>,TEntity> where TDbContext : DbContext where TEntity : BaseEntity
    {
        private readonly TDbContext _dbContext;

        public GetEntityRequestHandler(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TEntity> Handle(GetEntityRequest<TEntity> request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Set<TEntity>().FirstOrDefaultAsync(p => p.Id == request.Id);
            if (entity == null)
            {
                throw new NotFoundException(nameof(TEntity), request.Id, "The resource is not found");
            }
            return entity;
        }

    }
}
