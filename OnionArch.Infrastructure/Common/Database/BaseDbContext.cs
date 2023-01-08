using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using OnionArch.Domain.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OnionArch.Domain.Common.Tenant;
using OnionArch.Domain.ProductInventory;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace OnionArch.Infrastructure.Common.Database
{
    public class BaseDbContext<TDbContext> : DbContext
    {
        private readonly ICurrentTenant _currentTenant;
        private readonly IMediator _mediator;

        public BaseDbContext(
           DbContextOptions options,ICurrentTenant currentTenant, IMediator mediator)
           : base(options)
        {
            _currentTenant = currentTenant;
            _mediator = mediator;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //加载配置
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TDbContext).Assembly);

            //配置每个继承AggregateRoot<BaseEntity>的实体
            var setAggregateRootConfig = typeof(TDbContext).GetMethod("SetAggregateRootConfig", BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var entityType in GetAggregateRootTypes(modelBuilder))
            {
                var method = setAggregateRootConfig.MakeGenericMethod(entityType);
                method.Invoke(this, new object[] { modelBuilder, entityType });
            }

            //配置每个DomainEventEntity<BaseEntity>实体
            var setDomainEventEntityConfigMethod = typeof(TDbContext).GetMethod("SetDomainEventEntityConfig", BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var entityType in GetDomainEventEntityTypes(modelBuilder))
            {
                var method = setDomainEventEntityConfigMethod.MakeGenericMethod(entityType);
                method.Invoke(this, new object[] { modelBuilder, entityType });
            }
        }


        private static IList<Type> _aggregateRootTypesCache;
        private IList<Type> GetAggregateRootTypes(ModelBuilder modelBuilder)
        {
            if (_aggregateRootTypesCache != null)
            {
                return _aggregateRootTypesCache.ToList();
            }

            _aggregateRootTypesCache = modelBuilder.Model.GetEntityTypes().Where(t => t.ClrType.BaseType == typeof(AggregateRoot<BaseEntity>)).Select(t => t.ClrType).ToList();

            return _aggregateRootTypesCache;
        }

        // This method is called for every loaded entity type in OnModelCreating method.
        // Here type is known through generic parameter and we can use EF Core methods.
        private void SetAggregateRootConfig<T>(ModelBuilder builder, Type entityType) where T : AggregateRoot<T>
        {
            builder.Entity<T>().ToTable("T_" + entityType.Name);
            builder.Entity<T>().HasKey(e => e.Id);
            builder.Entity<T>().HasIndex(e => new { e.Id, e.TenantId });
            builder.Entity<T>().HasIndex(e => e.TenantId);
            builder.Entity<T>().HasQueryFilter(e => e.TenantId == _currentTenant.TenantId);
            builder.Entity<T>().HasMany(g => g.DomainEvents).WithOne(s => s.AggregateRootEntity).HasForeignKey(s => s.AggregateRootId);
        }

        private static IList<Type> _domainEventEntityTypesCache;
        private IList<Type> GetDomainEventEntityTypes(ModelBuilder modelBuilder)
        {
            if (_domainEventEntityTypesCache != null)
            {
                return _domainEventEntityTypesCache.ToList();
            }

            _domainEventEntityTypesCache = modelBuilder.Model.GetEntityTypes().Where(t => t.ClrType == typeof(DomainEventEntity<BaseEntity>)).Select(t => t.ClrType).ToList();

            return _domainEventEntityTypesCache;
        }

        // This method is called for every loaded entity type in OnModelCreating method.
        // Here type is known through generic parameter and we can use EF Core methods.
        private void SetDomainEventEntityConfig<T>(ModelBuilder builder, Type entityType) where T : DomainEventEntity<T>
        {
            builder.Entity<T>().ToTable("T_" + entityType.Name);
            builder.Entity<T>().HasKey(e => e.Id);
            builder.Entity<T>().HasIndex(e => new { e.Id, e.TenantId });
            builder.Entity<T>().HasIndex(e => e.TenantId);
            builder.Entity<T>().HasQueryFilter(e => e.TenantId == _currentTenant.TenantId);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            throw new NotImplementedException("Do not call SaveChanges, please call SaveChangesAsync instead.");
        }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var domainRootEntities = ChangeTracker.Entries<AggregateRoot<BaseEntity>>();
            //为每个继承AggregateRooteEntity的实体的Id主键和TenantId赋值
            foreach (var entry in domainRootEntities)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        if (entry.Entity.Id == Guid.Empty)
                            entry.Entity.Id = Guid.NewGuid();
                        if (entry.Entity.TenantId == Guid.Empty)
                            entry.Entity.TenantId = _currentTenant.TenantId;
                        break;
                }
            }

            //所有包含领域事件的领域跟实体
            var haveEventEntities = domainRootEntities.Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any()).ToList();
            //所有的领域事件
            var domainEvents = haveEventEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();
           
            //清除所有领域根实体的领域事件
            haveEventEntities
                .ForEach(entity => entity.Entity.ClearDomainEvents());
            //生成领域事件任务并执行
            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    await _mediator.Publish(domainEvent);
                });
            await Task.WhenAll(tasks);

            return await base.SaveChangesAsync(cancellationToken);
        }


    }
}
