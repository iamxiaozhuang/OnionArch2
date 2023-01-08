using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Domain.Common.Database
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
