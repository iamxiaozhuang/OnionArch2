using MediatR;

namespace OnionArch.Domain.Common.Repositories
{

    public class CreateEntityRequest<TEntity> : IRequest<TEntity>
    {

        public CreateEntityRequest(TEntity entity) 
        {
            Entity = entity;
        }

        public TEntity Entity { get; }
    }
}
