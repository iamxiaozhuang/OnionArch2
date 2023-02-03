using MediatR;
using OnionArch.Application.Common.CQRS;
using OnionArch.Application.Common.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.ProductInventoryUseCase
{
    [MediatorWebAPIConfig(HttpMethod = HttpMethodToGenerate.Post, HttpUrl = "/productinventory", Summary = "创建产品库存", Description = "创建产品库存 Description")]
    public class CreateProductInventory : ICommand<Unit>
    {
        public string ProductCode { get; set; }
        public int InventoryAmount { get; set; }
    }
    [MediatorWebAPIConfig(HttpMethod = HttpMethodToGenerate.Patch, HttpUrl = "/productinventory/increase", Summary = "增加产品库存")]
    public class IncreaseProductInventory : ICommand<Unit>
    {
        public Guid Id { get; set; }
        public int Amount { get; set; }
    }
    [MediatorWebAPIConfig(HttpMethod = HttpMethodToGenerate.Patch, HttpUrl = "/productinventory/decrease", Summary = "减少产品库存")]
    public class DecreaseProductInventory : ICommand<Unit>
    {
        public string ProductCode { get; set; }
        public int Amount { get; set; }
    }
    [MediatorWebAPIConfig(HttpMethod = HttpMethodToGenerate.Get, HttpUrl = "/productinventory", Summary = "获取产品库存", Description = "")]
    public class ReadProductInventory : IQuery<TestResult>
    {
        public Guid Id { get; set; }
    }

    [MediatorWebAPIConfig(HttpMethod = HttpMethodToGenerate.Delete, HttpUrl = "/productinventory", Summary = "删除产品库存", Description = "")]
    public class DeleteProductInventory : ICommand<TestResult>
    {
        public Guid Id { get; set; }
    }

    [MediatorWebAPIConfig(HttpMethod = HttpMethodToGenerate.Get, HttpUrl = "/productinventory/list", Summary = "分页列出产品库存", Description = "")]
    public class PagedListProductInventory : IQuery<List<TestResult>>
    {
       public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }




    public class TestResult
    {
        public string Message { get; set; }
    }
}
