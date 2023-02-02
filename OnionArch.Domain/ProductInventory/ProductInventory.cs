using OnionArch.Domain.Common.Entities;

namespace OnionArch.Domain.ProductInventory
{
    public record ProductInventory(string ProductCode) : AggregateRootEntity
    {
        private ProductInventory() : this(string.Empty) { }

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
