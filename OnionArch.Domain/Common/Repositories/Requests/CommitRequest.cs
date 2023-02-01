
using MediatR;

namespace OnionArch.Domain.Common.Repositories
{
    public class CommitRequest : IRequest<int>
    {
        public CommitRequest(CancellationToken cancellationToken)
        {
            CancellationToken = cancellationToken;
        }

        public CancellationToken CancellationToken { get; set; }
    }
}
