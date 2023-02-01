using MediatR;

namespace OnionArch.Domain.Common.Repositories
{

    public class GetEntityRequest<TEntity> : IRequest<TEntity>
    {
        public GetEntityRequest(Guid id) 
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
