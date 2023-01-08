using Mapster;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using OnionArch.Application.Common.Behaviours;
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
            TypeAdapterConfig.GlobalSettings.Default.MapToConstructor(true);
            TypeAdapterConfig.GlobalSettings.Default.IgnoreNullValues(true);
            services.AddMediatR(Assembly.GetExecutingAssembly());


            return services;
        }
    }
}
