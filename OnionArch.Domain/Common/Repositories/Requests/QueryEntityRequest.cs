using Mapster;
using MediatR;

namespace OnionArch.Domain.Common.Repositories
{

    public class QueryEntityRequest<TEntity, TModel> : IRequest<TModel>
    {
        public QueryEntityRequest(Guid id) 
        {
            Id = id;
        }

        public Guid Id { get; set; }

    }
}
