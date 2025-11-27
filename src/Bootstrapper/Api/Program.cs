using Carter;
using Serilog;
using Shared.Exceptions.Handler;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));


var catalogAssembly = typeof(CatalogModule).Assembly;
var basketAssembly = typeof(CatalogModule).Assembly;

builder.Services.AddCarterWithAssemblies(catalogAssembly, basketAssembly);
builder.Services.AddMediatrWithAssemblies(basketAssembly, catalogAssembly);

// Add services to the container
builder.Services
       .AddCatalogModule(builder.Configuration)
       .AddBasketModule(builder.Configuration)
       .AddOrderingModule(builder.Configuration);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

app.MapCarter();
app.UseSerilogRequestLogging();
app.UseExceptionHandler(options =>
{

});

// Configure the HTTP request pipeline
app
    .UseCatalogModule()
    .UseBasketModule()
    .UseOrderingModule();



app.Run();
