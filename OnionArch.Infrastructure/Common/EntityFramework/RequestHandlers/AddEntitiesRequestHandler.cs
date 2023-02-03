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
    public class AddEntitiesRequestHandler<TDbContext,TEntity> : IRequestHandler<AddEntitiesRequest<TEntity>> where TDbContext : DbContext where TEntity : BaseEntity
    {
        private readonly TDbContext _dbContext;

        public AddEntitiesRequestHandler(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(AddEntitiesRequest<TEntity> request, CancellationToken cancellationToken)
        {
            await _dbContext.Set<TEntity>().AddRangeAsync(request.Entities);
            return Unit.Value;
        }

    }
}
