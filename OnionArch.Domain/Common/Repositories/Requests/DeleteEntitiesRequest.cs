using MediatR;
using System.Linq.Expressions;

namespace OnionArch.Domain.Common.Repositories
{

    public class DeleteEntitiesRequest<TEntity> : IRequest<int>
    {
        public DeleteEntitiesRequest(Expression<Func<TEntity, bool>> whereLambda) 
        {
            WhereLambda= whereLambda;
        }

        public Expression<Func<TEntity, bool>> WhereLambda { get; set; }

    }

}
