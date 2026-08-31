using System.Threading.RateLimiting;
using DotNet8_Enterprise_CRUD.Data;
using DotNet8_Enterprise_CRUD.Filters;
using DotNet8_Enterprise_CRUD.Middleware;
using DotNet8_Enterprise_CRUD.Repositories;
using DotNet8_Enterprise_CRUD.Services;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------------------------------
// Controllers + Custom Filter
// ----------------------------------------------------

builder.Services.AddControllers(options =>
{
    options.Filters.Add<RequestLoggingFilter>();
});

// ----------------------------------------------------
// Swagger / OpenAPI
// ----------------------------------------------------

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DotNet 8 Enterprise CRUD API",
        Version = "v1",
        Description =
            "Enterprise CRUD API demonstrating Repository Pattern, " +
            "Service Layer, EF Core, Custom Middleware, Custom Filters, " +
            "Dependency Injection Lifetimes and API Rate Limiting."
    });
});

// ----------------------------------------------------
// Entity Framework Core + SQL Server
// ----------------------------------------------------

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// ----------------------------------------------------
// Repository Pattern
// ----------------------------------------------------

builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddTransient<ITransientService, TransientService>();

builder.Services.AddScoped<IScopedService, ScopedService>();

builder.Services.AddSingleton<ISingletonService, SingletonService>();

// ----------------------------------------------------
// API Rate Limiting
// ----------------------------------------------------

var rateLimitConfig = builder.Configuration.GetSection("RateLimit");

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("api", limiterOptions =>
    {
        limiterOptions.PermitLimit = 2;
        limiterOptions.Window = TimeSpan.FromSeconds(30);
        limiterOptions.QueueLimit = 0;
        limiterOptions.QueueProcessingOrder =
            QueueProcessingOrder.OldestFirst;
        limiterOptions.AutoReplenishment = true;
    });

    options.RejectionStatusCode =
        StatusCodes.Status429TooManyRequests;

    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.ContentType =
            "application/json";

        await context.HttpContext.Response.WriteAsJsonAsync(
            new
            {
                statusCode = 429,
                message = "Too many requests. Please try again later."
            },
            cancellationToken);
    };
});
// ----------------------------------------------------
// Build Application
// ----------------------------------------------------

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseMiddleware<RequestTimingMiddleware>();

app.UseHttpsRedirection();

app.UseRouting();

app.UseRateLimiter();

app.UseAuthorization();

app.MapControllers();

app.Run();