using Mapster;
using MediatR;
using OnionArch.Domain.Common.Database;
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

        public async Task<Unit> Handle(CreateProductInventory request, CancellationToken cancellationToken)
        {
            ProductInventory entity = request.Adapt<ProductInventory>();
            entity.Create();
            return Unit.Value;
        }
    }

    public class ReadProductInventoryHandler : IRequestHandler<ReadProductInventory, ReadProductInventoryResult>
    {
        private readonly RepositoryService<ProductInventory> _repositoryService;

        public ReadProductInventoryHandler(RepositoryService<ProductInventory> repositoryService)
        {
            _repositoryService = repositoryService;
        }

        public async Task<ReadProductInventoryResult> Handle(ReadProductInventory request, CancellationToken cancellationToken)
        {
            ProductInventory productInventory = await _repositoryService.ReadEntity(request.Id);
            ReadProductInventoryResult testResponseMessage = new ReadProductInventoryResult();
            testResponseMessage.Message = $"Inventory:{productInventory.ProductCode},{productInventory.InventoryAmount}";
            return testResponseMessage;
        }
    }
}
