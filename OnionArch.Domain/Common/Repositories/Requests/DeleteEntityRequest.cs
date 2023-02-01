using MediatR;

namespace OnionArch.Domain.Common.Repositories
{

    public class DeleteEntityRequest<TEntity> : IRequest<TEntity>
    {
        public DeleteEntityRequest(Guid id) 
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
