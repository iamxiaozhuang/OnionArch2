using MediatR;
using OnionArch.Domain.Common.Paged;
using System.Linq.Expressions;

namespace OnionArch.Domain.Common.Repositories
{

    public class QueryPagedEntitiesRequest<TEntity, TOrder> : IRequest<PagedResult<TEntity>>
    {
        public QueryPagedEntitiesRequest(Expression<Func<TEntity, bool>> whereLambda, PagedOption pagedOption, Expression<Func<TEntity, TOrder>> orderbyLambda, bool isAsc) 
        {
            WhereLambda= whereLambda;
            PagedOption = pagedOption;
            OrderbyLambda= orderbyLambda;
            IsAsc= isAsc;
        }

        public Expression<Func<TEntity, bool>> WhereLambda { get; set; }

        public PagedOption PagedOption { get; set; }

        public Expression<Func<TEntity, TOrder>> OrderbyLambda { get; set; }

        public bool IsAsc { get;set; }
    }

}
