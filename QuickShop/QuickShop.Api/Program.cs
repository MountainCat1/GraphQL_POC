using QuickShop.Api;
using QuickShop.Api.Extensions;
using QuickShop.Api.Extensions.ServiceCollection;

var builder = WebApplication.CreateBuilder(args);

// ========= CONFIGURATION  =========
var configuration = builder.Configuration;

// ========= SERVICES  =========
var services = builder.Services;

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
});

services.AddGraphQLServer().AddQueryType<Query>();

// ========= RUN  =========
var app = builder.Build();

if (!app.Environment.IsDevelopment())
    await app.MigrateDatabaseAsync<QuickShopDbContext>();

// if (app.Environment.IsDevelopment() || app.Configuration.GetValue<bool>("ENABLE_SWAGGER"))
// {
//     app.UseSwagger();
//     app.UseSwaggerUI(c =>
//     {
//         c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
//
//         // Enable JWT authentication in Swagger UI
//         c.OAuthClientId("swagger");
//         c.OAuthAppName("Swagger UI");
//     });
// }

// app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseCors("AllowOrigins");

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

#pragma warning disable ASP0014
app.UseEndpoints(x =>
{
    x.MapGraphQL();
});
#pragma warning restore ASP0014

app.Run();