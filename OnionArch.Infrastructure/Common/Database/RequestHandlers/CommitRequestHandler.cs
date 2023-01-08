using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArch.Domain.Common.Base;
using OnionArch.Domain.Common.Database;
using OnionArch.Domain.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Infrastructure.Common.Database.RequestHandlers
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
