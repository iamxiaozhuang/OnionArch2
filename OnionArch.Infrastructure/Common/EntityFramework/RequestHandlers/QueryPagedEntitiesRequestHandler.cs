using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArch.Domain.Common.Entities;
using OnionArch.Domain.Common.Exceptions;
using OnionArch.Domain.Common.Paged;
using OnionArch.Domain.Common.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Infrastructure.Common.EntityFramework.RequestHandlers
{
    public class QueryPagedEntitiesRequestHandler<TDbContext,TEntity,TOrder,TModel> : IRequestHandler<QueryPagedEntitiesRequest<TEntity, TOrder, TModel>, PagedResult<TModel>> where TDbContext : DbContext where TEntity : BaseEntity
    {
        private readonly TDbContext _dbContext;

        public QueryPagedEntitiesRequestHandler(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResult<TModel>> Handle(QueryPagedEntitiesRequest<TEntity, TOrder, TModel> request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Set<TEntity>().AsNoTracking();
            if (request.WhereLambda != null)
            {
                query = query.Where(request.WhereLambda);
            }
            if (request.OrderbyLambda != null)
            {
                if (request.IsAsc)
                    query = query.OrderBy(request.OrderbyLambda);
                else
                    query = query.OrderByDescending(request.OrderbyLambda);
            }
            PagedResult<TModel> result = new PagedResult<TModel>();
            result.PageNumber = request.PagedOption.PageNumber;
            result.PageSize = request.PagedOption.PageSize;
            result.Count = await query.CountAsync();
            query  = query.Skip((result.PageNumber - 1) * result.PageSize).Take(result.PageSize);
            result.QueryableData = query.ProjectToType<TModel>();
            return result;
        }

    }
}
