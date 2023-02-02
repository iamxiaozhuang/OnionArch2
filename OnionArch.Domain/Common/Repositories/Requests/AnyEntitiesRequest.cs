using MediatR;
using System.Linq.Expressions;

namespace OnionArch.Domain.Common.Repositories
{

    public class AnyEntitiesRequest<TEntity> : IRequest<bool>
    {
        public AnyEntitiesRequest(Expression<Func<TEntity, bool>> whereLambda)
        {
            WhereLambda = whereLambda;
        }

        public Expression<Func<TEntity, bool>> WhereLambda { get; set; }
    }

}
