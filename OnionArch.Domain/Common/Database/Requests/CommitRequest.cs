
using MediatR;

namespace OnionArch.Domain.Common.Database
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
