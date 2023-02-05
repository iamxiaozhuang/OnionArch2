using Mapster;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using OnionArch.Domain.Common.Repositories;
using OnionArch.Domain.ProductInventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
