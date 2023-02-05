using Mapster;
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
    public class QueryEntityRequestHandler<TDbContext,TEntity,TModel> : IRequestHandler<QueryEntityRequest<TEntity,TModel>, TModel> where TDbContext : DbContext where TEntity : BaseEntity
    {
        private readonly TDbContext _dbContext;

        public QueryEntityRequestHandler(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TModel> Handle(QueryEntityRequest<TEntity, TModel> request, CancellationToken cancellationToken)
        {
            var model = await _dbContext.Set<TEntity>().AsNoTracking().Where(p => p.Id == request.Id).ProjectToType<TModel>().FirstOrDefaultAsync();
            if (model == null)
            {
                throw new NotFoundException(nameof(TEntity), request.Id, "The resource is not found");
            }
            return model;
        }

    }
}
