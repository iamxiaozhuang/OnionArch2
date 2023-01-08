using Mapster;
using OnionArch.Domain.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Domain.ProductInventory
{
    public record ProductInventory(string ProductCode, int InventoryAmount) : AggregateRoot<ProductInventory>
    {
        private ProductInventory() : this(string.Empty, 0) { }

        public static ProductInventory Create<TModel>(TModel model)
        {
            ProductInventory entity = model.Adapt<ProductInventory>();
            entity.Create();
            return entity;
        }

    }
}
