using CleanArchitecture2025.Application;
using CleanArchitecture2025.Infrastructure;
using CleanArchitecture2025.WebAPI;
using CleanArchitecture2025.WebAPI.Controllers;
using CleanArhictecture2025.WebAPI;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.RateLimiting;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddResponseCompression(opt =>
{
    opt.EnableForHttps = true;
});

// builder.Services.AddApplicationMetaData()
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCors();
builder.Services.AddOpenApi();

builder.Services.AddControllers().AddOData(options => {

    options
        .EnableQueryFeatures()
        .AddRouteComponents("odata", AppODataController.GetEdmModel());
    
});

builder.Services.AddRateLimiter(x =>
    x.AddFixedWindowLimiter("fixed", cfg =>
    {
        cfg.QueueLimit = 100;
        cfg.Window = TimeSpan.FromSeconds(1);
        cfg.PermitLimit = 100;
        cfg.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    }));

builder.Services.AddExceptionHandler<ExceptionHandler>().AddProblemDetails();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

app.MapControllers().RequireRateLimiting("fixed").RequireAuthorization();

app.UseHttpsRedirection();

app.UseCors(x => x
    .AllowAnyHeader()
    .AllowCredentials()
    .AllowAnyMethod()
    .SetIsOriginAllowed(t => true));

app.UseExceptionHandler();

app.RegisterRoutes();

app.UseAuthentication();
app.UseAuthorization();

ExtensionsMiddleware.CreateFirstUser(app);

app.UseResponseCompression();

app.Run();
