using MediatR;
using OnionArch.Domain.Common.Entities;
using System.Linq.Expressions;

namespace OnionArch.Domain.Common.Repositories
{

    public class EditEntitiesRequest<TEntity> : IRequest<IQueryable<TEntity>>
    {
        public EditEntitiesRequest(Expression<Func<TEntity, bool>> whereLambda)
        {
            WhereLambda = whereLambda;
        }

        public Expression<Func<TEntity, bool>> WhereLambda { get; set; }
    }

}
