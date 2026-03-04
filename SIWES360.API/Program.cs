using MediatR;
using Serilog;
using SIWES360.Application;
using SIWES360.Infrastructure;
using SIWES360.Infrastructure.Persistence;
using Scalar.AspNetCore;
using SIWES360.Application.Common.Interfaces;
using SIWES360.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day);
});
builder.Services.AddOpenApi();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();



var app = builder.Build();

await AppDbSeeder.SeedAsync(app.Services);

// Configure the HTTP request pipeline.

app.MapOpenApi();

app.MapScalarApiReference(options =>
{
 options
     .WithTitle("SIWES360 API Docs")
     .WithTheme(ScalarTheme.Mars);
});
app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

