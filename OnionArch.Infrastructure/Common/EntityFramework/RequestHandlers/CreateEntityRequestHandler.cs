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
    public class CreateEntityRequestHandler<TDbContext,TEntity> : IRequestHandler<CreateEntityRequest<TEntity>,TEntity> where TDbContext : DbContext where TEntity : BaseEntity
    {
        private readonly TDbContext _dbContext;

        public CreateEntityRequestHandler(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TEntity> Handle(CreateEntityRequest<TEntity> request, CancellationToken cancellationToken)
        {
            var entry = await _dbContext.Set<TEntity>().AddAsync(request.Entity);
            return entry.Entity;
        }

    }
}
