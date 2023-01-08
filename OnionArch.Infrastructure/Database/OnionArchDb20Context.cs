using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArch.Domain.Common.Base;
using OnionArch.Domain.Common.Tenant;
using OnionArch.Domain.ProductInventory;
using OnionArch.Infrastructure.Common.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Infrastructure.Database
{
    public class OnionArchDb20Context : BaseDbContext<OnionArchDb20Context>
    {
        public OnionArchDb20Context(
            DbContextOptions<OnionArchDb20Context> options,ICurrentTenant currentTenant, IMediator mediator)
            : base(options, currentTenant, mediator)
        {
        }

        public DbSet<ProductInventory> ProductInventory { get; set; }

        public DbSet<DomainEventEntity<ProductInventory>> ProductInventoryDomainEvents { get; set; }


    }
}
