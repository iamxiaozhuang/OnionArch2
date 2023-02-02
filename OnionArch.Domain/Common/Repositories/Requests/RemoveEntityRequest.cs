using MediatR;

namespace OnionArch.Domain.Common.Repositories
{

    public class RemoveEntityRequest<TEntity> : IRequest<TEntity>
    {
        public RemoveEntityRequest(Guid id) 
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
