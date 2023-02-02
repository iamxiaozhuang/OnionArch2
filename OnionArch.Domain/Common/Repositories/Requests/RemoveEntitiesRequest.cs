using MediatR;
using System.Linq.Expressions;

namespace OnionArch.Domain.Common.Repositories
{

    public class RemoveEntitiesRequest<TEntity> : IRequest<int>
    {
        public RemoveEntitiesRequest(Expression<Func<TEntity, bool>> whereLambda) 
        {
            WhereLambda= whereLambda;
        }

        public Expression<Func<TEntity, bool>> WhereLambda { get; set; }

    }

}
