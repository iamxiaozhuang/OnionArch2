using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnionArch.Domain.Common.Base;
using OnionArch.Domain.Common.Database;
using OnionArch.Domain.ProductInventory;
using OnionArch.Infrastructure.Common.Database.NotificationHandlers;
using OnionArch.Infrastructure.Common.Database.RequestHandlers;
using OnionArch.Infrastructure.Database;
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
            services.AddTransient<IRequestHandler<CommitRequest, int>, CommitRequestHandler<OnionArchDb20Context>>();
            services.AddTransient<INotificationHandler<DomainEventEntity<ProductInventory>>, DomainEventEntityHandler<OnionArchDb20Context, ProductInventory>>();
            services.AddTransient<RepositoryService<ProductInventory>>();
            services.AddTransient<IRequestHandler<ReadEntityRequest<ProductInventory>, ProductInventory>, ReadEntityRequestHandler<OnionArchDb20Context, ProductInventory>>();



            return services;
        }
    }
}