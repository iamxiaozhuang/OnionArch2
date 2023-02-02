using Mapster;
using MediatR;
using OnionArch.Domain.Common.Repositories;
using OnionArch.Domain.ProductInventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.ProductInventoryUseCase
{
    public class CreateProductInventoryHandler : IRequestHandler<CreateProductInventory>
    {
        private readonly RepositoryService<ProductInventory> _repositoryService;

        public CreateProductInventoryHandler(RepositoryService<ProductInventory> repositoryService)
        {
            _repositoryService = repositoryService;
        }

        public async Task<Unit> Handle(CreateProductInventory request, CancellationToken cancellationToken)
        {
            ProductInventory entity = request.Adapt<ProductInventory>();
            await _repositoryService.Add(entity);
            return Unit.Value;
        }
    }

    public class ReadProductInventoryHandler : IRequestHandler<ReadProductInventory, TestResult>
    {
        private readonly RepositoryService<ProductInventory> _repositoryService;

        public ReadProductInventoryHandler(RepositoryService<ProductInventory> repositoryService)
        {
            _repositoryService = repositoryService;
        }

        public async Task<TestResult> Handle(ReadProductInventory request, CancellationToken cancellationToken)
        {
            ProductInventory productInventory = await _repositoryService.Query(request.Id);
            TestResult testResponseMessage = new TestResult();
            testResponseMessage.Message = $"Get Product Inventory:{productInventory.ProductCode},{productInventory.InventoryAmount}";
            return testResponseMessage;
        }
    }

    public class DeleteProductInventoryHandler : IRequestHandler<DeleteProductInventory, TestResult>
    {
        private readonly RepositoryService<ProductInventory> _repositoryService;

        public DeleteProductInventoryHandler(RepositoryService<ProductInventory> repositoryService)
        {
            _repositoryService = repositoryService;
        }

        public async Task<TestResult> Handle(DeleteProductInventory request, CancellationToken cancellationToken)
        {
            ProductInventory productInventory = await _repositoryService.Remove(request.Id);
            TestResult testResponseMessage = new TestResult();
            testResponseMessage.Message = $"Delete successfully:{productInventory.ProductCode},{request.Id}";
            return testResponseMessage;
        }
    }

    public class IncreaseProductInventoryHandler : IRequestHandler<IncreaseProductInventory>
    {
        private readonly RepositoryService<ProductInventory> _repositoryService;

        public IncreaseProductInventoryHandler(RepositoryService<ProductInventory> repositoryService)
        {
            _repositoryService = repositoryService;
        }

        public async Task<Unit> Handle(IncreaseProductInventory request, CancellationToken cancellationToken)
        {
            ProductInventory productInventory = await _repositoryService.Edit(request.Id);
            productInventory.IncreaseInventory(request.Amount);
            return Unit.Value;
        }
    }


    public class DecreaseProductInventoryHandler : IRequestHandler<DecreaseProductInventory>
    {
        private readonly RepositoryService<ProductInventory> _repositoryService;

        public DecreaseProductInventoryHandler(RepositoryService<ProductInventory> repositoryService)
        {
            _repositoryService = repositoryService;
        }

        public async Task<Unit> Handle(DecreaseProductInventory request, CancellationToken cancellationToken)
        {
            var query = await _repositoryService.Edit(p => p.ProductCode == request.ProductCode);
            ProductInventory productInventory = query.First();
            productInventory.DecreaseInventory(request.Amount);
            return Unit.Value;
        }
    }
}
