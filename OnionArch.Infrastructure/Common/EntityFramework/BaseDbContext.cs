using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OnionArch.Domain.ProductInventory;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OnionArch.Domain.Common.Entities;
using Newtonsoft.Json;
using OnionArch.Domain.Common.DomainEvents;
using Newtonsoft.Json.Linq;
using OnionArch.Domain.Common.CurrentContext;

namespace OnionArch.Infrastructure.Common.Database
{
    public class BaseDbContext<TDbContext> : DbContext
    {
        private readonly ICurrentContext _currentContext;
        private readonly IMediator _mediator;

        public BaseDbContext(
           DbContextOptions options, ICurrentContext currentContext, IMediator mediator)
           : base(options)
        {
            _currentContext = currentContext;
            _mediator = mediator;
        }

        public DbSet<EntityChangedAuditEntity> EntityChangedAuditLog { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //加载配置
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TDbContext).Assembly);

            //配置每个继承AggregateRootEntity<BaseEntity>的实体
            var setBaseEntityConfig = typeof(TDbContext).GetMethod("SetBaseEntityConfig", BindingFlags.Public | BindingFlags.Instance);
            foreach (var entityType in GetBaseEntityTypes(modelBuilder))
            {
                var method = setBaseEntityConfig.MakeGenericMethod(entityType);
                method.Invoke(this, new object[] { modelBuilder });
            }
        }


        private static IList<Type> _baseEntityTypesCache;
        private IList<Type> GetBaseEntityTypes(ModelBuilder modelBuilder)
        {
            if (_baseEntityTypesCache != null)
            {
                return _baseEntityTypesCache.ToList();
            }

            _baseEntityTypesCache = modelBuilder.Model.GetEntityTypes().Where(t => t.ClrType.BaseType == typeof(BaseEntity) || t.ClrType.BaseType?.BaseType == typeof(BaseEntity)).Select(t => t.ClrType).ToList();

            return _baseEntityTypesCache;
        }

        // This method is called for every loaded entity type in OnModelCreating method.
        // Here type is known through generic parameter and we can use EF Core methods.
        public void SetBaseEntityConfig<T>(ModelBuilder builder) where T : BaseEntity
        {
            builder.Entity<T>().HasKey(e => e.Id);
            builder.Entity<T>().HasIndex(e => new { e.Id, e.TenantId });
            builder.Entity<T>().HasIndex(e => e.TenantId);
            builder.Entity<T>().HasQueryFilter(e => e.TenantId == _currentContext.TenantId);
        }

       

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            throw new NotImplementedException("Do not call SaveChanges, please call SaveChangesAsync instead.");
        }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            //为每个继承BaseEntity的实体设置Id和TenantId的值
            var baseEntities = ChangeTracker.Entries<BaseEntity>();
            foreach (var entry in baseEntities)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        if (entry.Entity.Id == Guid.Empty)
                            entry.Entity.Id = Guid.NewGuid();
                        if (entry.Entity.TenantId == Guid.Empty)
                            entry.Entity.TenantId = _currentContext.TenantId;
                        break;
                }
            }

            //为每个继承AggregateRootEntity的实体生成和发布领域事件
            var aggregateRootEntities = ChangeTracker.Entries<AggregateRootEntity>();
            foreach (var entry in aggregateRootEntities)
            {
                var originalValues = new Dictionary<string, object?>();
                foreach (var property in entry.OriginalValues.Properties)
                {
                    originalValues.Add(property.Name, entry.OriginalValues[property]);
                }
                var currentValues = new Dictionary<string, object?>();
                foreach (var property in entry.CurrentValues.Properties)
                {
                    currentValues.Add(property.Name, entry.CurrentValues[property]);
                }
                var entityChangedDomainEvent =  new EntityChangedDomainEvent(entry.Entity, _currentContext.UserName, JsonConvert.SerializeObject(originalValues, Formatting.Indented), JsonConvert.SerializeObject(currentValues, Formatting.Indented));

                switch (entry.State)
                {
                    case EntityState.Added:
                        entityChangedDomainEvent.ChangeType = nameof(EntityState.Added);
                        break;
                    case EntityState.Modified:
                        entityChangedDomainEvent.ChangeType = nameof(EntityState.Modified);
                        break;
                    case EntityState.Deleted:
                        entityChangedDomainEvent.ChangeType = nameof(EntityState.Deleted);
                        break;
                }
                entry.Entity.AddDomainEvent(entityChangedDomainEvent);
            }

            //所有包含领域事件的领域跟实体
            var haveEventEntities = aggregateRootEntities.Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any()).ToList();
            //所有的领域事件
            var domainEvents = haveEventEntities.SelectMany(x => x.Entity.DomainEvents).ToList();
           
            //清除所有领域根实体的领域事件
            haveEventEntities.ForEach(entity => entity.Entity.ClearDomainEvents());
            //发布领域事件
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
