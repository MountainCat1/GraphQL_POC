using QuickShop.Api.Extensions;
using QuickShop.Api.Extensions.ServiceCollection;
using QuickShop.Application;
using QuickShop.Application.Services;
using QuickShop.Infrastructure.Contexts;
using Catut.Application.Configuration;
using Catut.Application.MediaRBehaviors;
using Catut.Application.Middlewares;
using Catut.Infrastructure.Abstractions;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// ========= CONFIGURATION  =========
var configuration = builder.Configuration;

configuration.AddJsonFile("Secrets/jwt.json");

var jwtConfig = configuration.GetConfiguration<JwtConfig>();
var apiConfig = configuration.GetConfiguration<ApiConfiguration>();

// ========= SERVICES  =========
var services = builder.Services;

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
    loggingBuilder.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
});

//  === INSTALLERS ===
services.InstallSwagger();
services.InstallCors();
services.InstallDbContext(configuration);
//  ===            ===

services.AddAsymmetricAuthentication(jwtConfig);

services.AddHttpContextAccessor();
services.AddTransient<IUserAccessor, UserAccessor>();
services.AddScoped<IDatabaseErrorMapper, DatabaseErrorMapper>();
services.AddScoped<ErrorHandlingMiddleware>();
services.AddFluentValidationAutoValidation();
services.AddValidatorsFromAssemblyContaining<ApplicationAssemblyMarker>();
services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(ApplicationAssemblyMarker).Assembly));
services.AddAuthorizationHandlers();

// ========= RUN  =========
var app = builder.Build();

if (!app.Environment.IsDevelopment())
    await app.MigrateDatabaseAsync<QuickShopDbContext>();

if (app.Environment.IsDevelopment() || app.Configuration.GetValue<bool>("ENABLE_SWAGGER"))
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");

        // Enable JWT authentication in Swagger UI
        c.OAuthClientId("swagger");
        c.OAuthAppName("Swagger UI");
    });
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseCors("AllowOrigins");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();