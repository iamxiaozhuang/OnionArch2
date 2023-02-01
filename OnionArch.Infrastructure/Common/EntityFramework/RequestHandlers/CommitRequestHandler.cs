using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArch.Domain.Common.Repositories;

namespace OnionArch.Infrastructure.Common.EntityFramework.RequestHandlers
{
    public class CommitRequestHandler<TDbContext> : IRequestHandler<CommitRequest,int> where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;

        public CommitRequestHandler(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(CommitRequest request, CancellationToken cancellationToken)
        {
            return await _dbContext.SaveChangesAsync(request.CancellationToken);
        }
    }
}
