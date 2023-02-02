using MediatR;

namespace OnionArch.Domain.Common.Repositories
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
            return await _mediator.Send(new SaveChangesRequest(cancellationToken));
        }
    }
}
