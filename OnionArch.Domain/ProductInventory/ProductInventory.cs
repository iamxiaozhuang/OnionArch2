using OnionArch.Domain.Common.Entities;

namespace OnionArch.Domain.ProductInventory
{
    public record ProductInventory(string ProductCode, int InventoryAmount) : AggregateRootEntity
    {
        private ProductInventory() : this(string.Empty, 0) { }

    }
}
