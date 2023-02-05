using Mapster;
using MediatR;
using OnionArch.Domain.Common.Entities;
using OnionArch.Domain.Common.Exceptions;
using OnionArch.Domain.Common.Paged;
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
            if( await _repositoryService.Any(p => p.ProductCode == request.ProductCode))
            {
                throw new BrokenBusinessRuleException("产品库存", request.ProductCode, "产品编码重复");
            }
            ProductInventory entity = ProductInventory.Create(request);
            await _repositoryService.Add(entity);
            return Unit.Value;
        }
    }

    public class ReadProductInventoryHandler : IRequestHandler<ReadProductInventory, ProductInventoryDto>
    {
        private readonly RepositoryService<ProductInventory> _repositoryService;

        public ReadProductInventoryHandler(RepositoryService<ProductInventory> repositoryService)
        {
            _repositoryService = repositoryService;
        }

        public async Task<ProductInventoryDto> Handle(ReadProductInventory request, CancellationToken cancellationToken)
        {
            TypeAdapterConfig<ProductInventory, ProductInventoryDto>.NewConfig().Map(d => d.Amount, s => s.InventoryAmount);
            ProductInventoryDto dto = await _repositoryService.Query<ProductInventoryDto>(request.Id);
            return dto;
        }
    }

    public class DeleteProductInventoryHandler : IRequestHandler<DeleteProductInventory, ProductInventoryDto>
    {
        private readonly RepositoryService<ProductInventory> _repositoryService;

        public DeleteProductInventoryHandler(RepositoryService<ProductInventory> repositoryService)
        {
            _repositoryService = repositoryService;
        }

        public async Task<ProductInventoryDto> Handle(DeleteProductInventory request, CancellationToken cancellationToken)
        {
            ProductInventory productInventory = await _repositoryService.Remove(request.Id);
            ProductInventoryDto dto = new ProductInventoryDto();
            dto.ProductCode = productInventory.ProductCode;
            dto.Amount = productInventory.InventoryAmount;
            return dto;
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


    public class PagedListProductInventoryHandler : IRequestHandler<PagedListProductInventory, List<ProductInventoryDto>>
    {
        private readonly RepositoryService<ProductInventory> _repositoryService;

        public PagedListProductInventoryHandler(RepositoryService<ProductInventory> repositoryService)
        {
            _repositoryService = repositoryService;
        }

        public async Task<List<ProductInventoryDto>> Handle(PagedListProductInventory request, CancellationToken cancellationToken)
        {
            var result = await _repositoryService.Query<string,ProductInventoryDto>(p => p.InventoryAmount > 0,
                new PagedOption() { PageNumber = request.PageNumber, PageSize = request.PageSize }, p => p.ProductCode);

            return result.QueryableData.ToList();
        }
    }
}
