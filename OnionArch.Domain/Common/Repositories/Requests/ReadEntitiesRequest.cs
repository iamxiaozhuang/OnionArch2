using MediatR;
using System.Linq.Expressions;

namespace OnionArch.Domain.Common.Repositories
{

    public class ReadEntitiesRequest<TEntity, TOrder> : IRequest<IQueryable<TEntity>>
    {
        public ReadEntitiesRequest(Expression<Func<TEntity, bool>> whereLambda, Expression<Func<TEntity, TOrder>> orderbyLambda, bool isAsc) 
        {
            WhereLambda= whereLambda;
            OrderbyLambda= orderbyLambda;
            IsAsc= isAsc;
        }

        public Expression<Func<TEntity, bool>> WhereLambda { get; set; }

        public Expression<Func<TEntity, TOrder>> OrderbyLambda { get; set; }

        public bool IsAsc { get;set; }
    }

}
