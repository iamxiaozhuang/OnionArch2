using MediatR;

namespace OnionArch.Domain.Common.Repositories
{

    public class AddEntityRequest<TEntity> : IRequest<TEntity>
    {

        public AddEntityRequest(TEntity entity) 
        {
            Entity = entity;
        }

        public TEntity Entity { get; }
    }
}
