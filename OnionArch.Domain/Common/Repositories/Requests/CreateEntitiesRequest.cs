using MediatR;

namespace OnionArch.Domain.Common.Repositories
{

    public class CreateEntitiesRequest<TEntity> : IRequest
    {

        public CreateEntitiesRequest(params TEntity[] entities) 
        {
            Entities = entities;
        }

        public TEntity[] Entities { get; }
    }
}
