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
    public class EditEntitiesRequestHandler<TDbContext,TEntity> : IRequestHandler<EditEntitiesRequest<TEntity>, IQueryable<TEntity>> where TDbContext : DbContext where TEntity : BaseEntity
    {
        private readonly TDbContext _dbContext;

        public EditEntitiesRequestHandler(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IQueryable<TEntity>> Handle(EditEntitiesRequest<TEntity> request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Set<TEntity>().AsQueryable();
            if (request.WhereLambda != null)
            {
                query = query.Where(request.WhereLambda);
            }
            return query;
        }

    }
}
