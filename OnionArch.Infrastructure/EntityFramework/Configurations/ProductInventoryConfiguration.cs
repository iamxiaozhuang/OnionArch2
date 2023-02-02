using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnionArch.Domain.ProductInventory;

namespace OnionArch.Infrastructure.EntityFramework.Configurations
{
    public class ProductInventoryConfiguration : IEntityTypeConfiguration<ProductInventory>
    {
        public void Configure(EntityTypeBuilder<ProductInventory> builder)
        {
            builder.Property(e => e.ProductCode)
               .IsRequired()
               .HasMaxLength(20);
            builder.HasIndex(e => new { e.ProductCode, e.TenantId }).IsUnique(true);
        }
    }
}
