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
    [MediatorWebAPIConfig(HttpMethod = HttpMethodToGenerate.Post, HttpUrl = "CreateProductInventory")]
    public class CreateProductInventory : ICommand<Unit>
    {
        public string ProductCode { get; set; }
        public int InventoryAmount { get; set; }
    }
    [MediatorWebAPIConfig(HttpMethod = HttpMethodToGenerate.Patch, HttpUrl = "IncreaseProductInventory")]
    public class IncreaseProductInventory : ICommand<Unit>
    {
        public Guid Id { get; set; }
        public int Amount { get; set; }
    }
    [MediatorWebAPIConfig(HttpMethod = HttpMethodToGenerate.Patch, HttpUrl = "DecreaseProductInventory")]
    public class DecreaseProductInventory : ICommand<Unit>
    {
        public string ProductCode { get; set; }
        public int Amount { get; set; }
    }
    [MediatorWebAPIConfig(HttpMethod = HttpMethodToGenerate.Get, HttpUrl = "ReadProductInventory")]
    public class ReadProductInventory : IQuery<TestResult>
    {
        public Guid Id { get; set; }
    }

    [MediatorWebAPIConfig(HttpMethod = HttpMethodToGenerate.Delete, HttpUrl = "DeleteProductInventory")]
    public class DeleteProductInventory : ICommand<TestResult>
    {
        public Guid Id { get; set; }
    }




    public class TestResult
    {
        public string Message { get; set; }
    }
}
