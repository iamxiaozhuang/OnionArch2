using Mapster;
using MediatR;
using OnionArch.Domain.Common.Entities;
using OnionArch.Domain.Common.Paged;
using System.Runtime.InteropServices;

namespace OnionArch.Domain.ProductInventory
{
    public record ProductInventory(string ProductCode) : AggregateRootEntity
    {
        private ProductInventory() : this(string.Empty) { }

        public static ProductInventory Create<TModel>(TModel model)
        {
            //var entity = new ProductInventory();
            var entity = model.Adapt<ProductInventory>();
            return entity;
        }

        public ProductInventory Update<TModel>(ProductInventory entity, TModel model)
        {
            model.Adapt(entity);
            return entity;
        }

        public int InventoryAmount { get; private set; }

        public void IncreaseInventory(int amount)
        {
            this.InventoryAmount += amount;
        }

        public void DecreaseInventory(int amount)
        {
            this.InventoryAmount -= amount;
        }

       
    }
}
