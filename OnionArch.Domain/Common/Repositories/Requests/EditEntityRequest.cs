using MediatR;

namespace OnionArch.Domain.Common.Repositories
{

    public class EditEntityRequest<TEntity> : IRequest<TEntity>
    {
        public EditEntityRequest(Guid id) 
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
