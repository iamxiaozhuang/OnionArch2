using MediatR;

namespace OnionArch.Domain.Common.Repositories
{

    public class AddEntitiesRequest<TEntity> : IRequest
    {

        public AddEntitiesRequest(params TEntity[] entities) 
        {
            Entities = entities;
        }

        public TEntity[] Entities { get; }
    }
}
