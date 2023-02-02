using MediatR;

namespace OnionArch.Domain.Common.Repositories
{

    public class QueryEntityRequest<TEntity> : IRequest<TEntity>
    {
        public QueryEntityRequest(Guid id) 
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
