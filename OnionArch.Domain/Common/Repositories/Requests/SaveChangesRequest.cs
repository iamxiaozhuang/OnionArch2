
using MediatR;

namespace OnionArch.Domain.Common.Repositories
{
    public class SaveChangesRequest : IRequest<int>
    {
        public SaveChangesRequest(CancellationToken cancellationToken)
        {
            CancellationToken = cancellationToken;
        }

        public CancellationToken CancellationToken { get; set; }
    }
}
