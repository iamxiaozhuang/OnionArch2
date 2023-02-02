using MediatR;
using System.Linq.Expressions;

namespace OnionArch.Domain.Common.Repositories
{

    public class QueryEntitiesRequest<TEntity> : IRequest<IQueryable<TEntity>>
    {
        public QueryEntitiesRequest(Expression<Func<TEntity, bool>> whereLambda) 
        {
            WhereLambda= whereLambda;
        }

        public Expression<Func<TEntity, bool>> WhereLambda { get; set; }
    }

}
