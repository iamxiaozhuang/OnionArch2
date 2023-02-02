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
        public string ProductCode { get; set; }
        public int InventoryAmount { get; set; }
    }
    [MediatorWebAPIConfig(HttpMethod = HttpMethodToGenerate.Patch, HttpUrl = "DecreaseProductInventory")]
    public class DecreaseInventory : ICommand<Unit>
    {
        public string ProductCode { get; set; }
        public int InventoryAmount { get; set; }
    }
    [MediatorWebAPIConfig(HttpMethod = HttpMethodToGenerate.Get, HttpUrl = "ReadProductInventory/{request}/")]
    public class ReadProductInventory : IQuery<ReadProductInventoryResult>
    {
        public Guid Id { get; set; }

        public static bool TryParse(string? value, out ReadProductInventory readProductInventory)
        {
            if(Guid.TryParse(value,out Guid id))
            {
                readProductInventory = new ReadProductInventory() { Id = id };
                return true;
            }
            readProductInventory = new ReadProductInventory();
            return false;
        }
    }
    public class ReadProductInventoryResult
    {
        public string Message { get; set; }
    }
}
