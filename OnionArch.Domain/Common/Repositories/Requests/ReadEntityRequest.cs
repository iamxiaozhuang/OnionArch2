using MediatR;

namespace OnionArch.Domain.Common.Repositories
{

    public class ReadEntityRequest<TEntity> : IRequest<TEntity>
    {
        public ReadEntityRequest(Guid id) 
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
