using MediatR.Pipeline;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnionArch.Domain.Common.Repositories;
using OnionArch.Application.Common.CQRS;

namespace OnionArch.Application.Common.Behaviours
{
    public class UnitOfWorkPostProcessor<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse> where TRequest : IRequest<TResponse>, ICommand<TResponse>
    {
        private readonly UnitOfWorkService _unitOfWorkService;

        public UnitOfWorkPostProcessor(UnitOfWorkService unitOfWorkService)
        {
            _unitOfWorkService = unitOfWorkService;
        }

        public async Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
        {
            await _unitOfWorkService.CommitAsync(cancellationToken);
        }
    }

}
