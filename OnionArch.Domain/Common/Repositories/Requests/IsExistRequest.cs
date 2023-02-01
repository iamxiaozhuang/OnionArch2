using MediatR;
using System.Linq.Expressions;

namespace OnionArch.Domain.Common.Repositories
{

    public class IsExistRequest<TEntity> : IRequest<bool>
    {
        public IsExistRequest(Expression<Func<TEntity, bool>> whereLambda)
        {
            WhereLambda = whereLambda;
        }

        public Expression<Func<TEntity, bool>> WhereLambda { get; set; }
    }

}
