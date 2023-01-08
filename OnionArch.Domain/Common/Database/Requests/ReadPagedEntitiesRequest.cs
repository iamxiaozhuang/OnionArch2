using MediatR;
using OnionArch.Domain.Common.Paged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Domain.Common.Database
{

    public class ReadPagedEntitiesRequest<TEntity, TOrder> : IRequest<PagedResult<TEntity>>
    {
        public ReadPagedEntitiesRequest(Expression<Func<TEntity, bool>> whereLambda, PagedOption pagedOption, Expression<Func<TEntity, TOrder>> orderbyLambda, bool isAsc) 
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
