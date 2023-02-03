using OnionArch.Application;
using OnionArch.Application.ProductInventoryUseCase;
using OnionArch.Domain.Common.CurrentContext;
using OnionArch.Infrastructure;
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
