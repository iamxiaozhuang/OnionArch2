using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnionArch.Domain.Common.DomainEvents;
using OnionArch.Domain.Common.Entities;
using OnionArch.Domain.Common.Repositories;
using OnionArch.Domain.ProductInventory;
using OnionArch.Infrastructure.Common.EntityFramework.NotificationHandlers;
using OnionArch.Infrastructure.Common.EntityFramework.RequestHandlers;
using OnionArch.Infrastructure.EntityFramework;
using System.Reflection;

namespace OnionArch.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            TypeAdapterConfig.GlobalSettings.Default.MapToConstructor(true);
            TypeAdapterConfig.GlobalSettings.Default.IgnoreNullValues(true);
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddDbContext<OnionArchDb20Context>(options =>
              options.UseNpgsql(configuration.GetConnectionString("OnionArchDB20")));

            services.AddTransient<UnitOfWorkService>();
            services.AddTransient<IRequestHandler<SaveChangesRequest, int>, SaveChangesRequestHandler<OnionArchDb20Context>>();

            services.AddTransient<RepositoryService<EntityChangedAuditEntity>>();
            services.AddTransient<IRequestHandler<AddEntityRequest<EntityChangedAuditEntity>, EntityChangedAuditEntity>, AddEntityRequestHandler<OnionArchDb20Context, EntityChangedAuditEntity>>();
            
            services.AddTransient<RepositoryService<ProductInventory>>();
            services.AddTransient<IRequestHandler<AddEntityRequest<ProductInventory>, ProductInventory>, AddEntityRequestHandler<OnionArchDb20Context, ProductInventory>>();
            services.AddTransient<IRequestHandler<QueryEntityRequest<ProductInventory>, ProductInventory>, QueryEntityRequestHandler<OnionArchDb20Context, ProductInventory>>();
            services.AddTransient<IRequestHandler<EditEntityRequest<ProductInventory>, ProductInventory>, EditEntityRequestHandler<OnionArchDb20Context, ProductInventory>>();
            services.AddTransient<IRequestHandler<EditEntitiesRequest<ProductInventory>, IQueryable<ProductInventory>>, EditEntitiesRequestHandler<OnionArchDb20Context, ProductInventory>>();
            services.AddTransient<IRequestHandler<RemoveEntityRequest<ProductInventory>, ProductInventory>, RemoveEntityRequestHandler<OnionArchDb20Context, ProductInventory>>();
           



            return services;
        }
    }
}