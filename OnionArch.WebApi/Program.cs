using Google.Api;
using MediatR;
using OnionArch.Application;
using OnionArch.Application.ProductInventoryUseCase;
using OnionArch.Domain.Common.CurrentContext;
using OnionArch.Domain.Common.Paged;
using OnionArch.Domain.Common.Repositories;
using OnionArch.Domain.ProductInventory;
using OnionArch.Infrastructure;
using OnionArch.Infrastructure.Common.EntityFramework.RequestHandlers;
using OnionArch.Infrastructure.EntityFramework;
using OnionArch.WebApi.Common;
using OnionArch.WebApi.Common.CurrentContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentContext, CurrentContext>();
//产品库存实体查询
builder.Services.AddTransient<IRequestHandler<QueryEntityRequest<ProductInventory, ProductInventoryDto>, ProductInventoryDto>, QueryEntityRequestHandler<OnionArchDb20Context, ProductInventory, ProductInventoryDto>>();
builder.Services.AddTransient<IRequestHandler<QueryPagedEntitiesRequest<ProductInventory, string, ProductInventoryDto>, PagedResult<ProductInventoryDto>>, QueryPagedEntitiesRequestHandler<OnionArchDb20Context, ProductInventory, string, ProductInventoryDto>>();


builder.Services.AddDaprClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapMediatorWebAPI(typeof(CreateProductInventory).Assembly);

app.Run();
