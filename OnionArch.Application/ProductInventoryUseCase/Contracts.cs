using MediatR;
using OnionArch.Application.Common.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.ProductInventoryUseCase
{
    public class CreateProductInventory : ICommand<Unit>
    {
        public string ProductCode { get; set; }
        public int InventoryAmount { get; set; }
    }

    public class ReadProductInventory : IQuery<ReadProductInventoryResult>
    {
        public Guid Id { get; set; }
    }
    public class ReadProductInventoryResult
    {
        public string Message { get; set; }
    }
}
