using Carter;
using Keycloak.AuthServices.Authentication;
using Serilog;
using Shared.Exceptions.Handler;
using Shared.Extensions;
using Shared.Messaging.Extensions;


var builder = WebApplication.CreateBuilder(args);


builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));


var catalogAssembly = typeof(CatalogModule).Assembly;
var basketAssembly = typeof(BasketModule).Assembly;
var orderingAssembly = typeof(OrderingModule).Assembly;



builder.Services.AddCarterWithAssemblies(catalogAssembly, basketAssembly, orderingAssembly);
builder.Services.AddMediatrWithAssemblies(catalogAssembly, basketAssembly, orderingAssembly);


builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddMassTransitWithAssemblies(builder.Configuration, catalogAssembly, basketAssembly);

builder.Services.AddKeycloakWebApiAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services
       .AddCatalogModule(builder.Configuration)
       .AddBasketModule(builder.Configuration)
       .AddOrderingModule(builder.Configuration);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

app.MapCarter();
app.UseSerilogRequestLogging();
app.UseExceptionHandler(options => { });
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline
app
    .UseCatalogModule()
    .UseBasketModule()
    .UseOrderingModule();



app.Run();
