using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Domain.Common.Database
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
