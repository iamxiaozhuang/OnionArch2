using MediatR;
using OnionArch.Domain.Common.Base;
using OnionArch.Domain.Common.Paged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Domain.Common.Database
{
    public class UnitOfWorkService
    {
        private readonly IMediator _mediator;

        public UnitOfWorkService(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// 提交数据库
        /// </summary>
        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(new CommitRequest(cancellationToken));
        }
    }
}
