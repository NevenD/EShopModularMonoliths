using Carter;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarterWithAssemblies(typeof(CatalogModule).Assembly);

// Add services to the container
builder.Services
       .AddCatalogModule(builder.Configuration)
       .AddBasketModule(builder.Configuration)
       .AddOrderingModule(builder.Configuration);

var app = builder.Build();

app.MapCarter();

// Configure the HTTP request pipeline
app
    .UseCatalogModule()
    .UseBasketModule()
    .UseOrderingModule();

app.UseExceptionHandler(exceptionHandler =>
{
    exceptionHandler.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        if (exception is null)
        {
            return;
        }

        var problemDetails = new ProblemDetails
        {
            Title = exception.Message,
            Status = StatusCodes.Status500InternalServerError,
            Detail = exception.StackTrace
        };

        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError("{exception}", exception.Message);

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsJsonAsync(problemDetails);
    });
});

app.Run();
