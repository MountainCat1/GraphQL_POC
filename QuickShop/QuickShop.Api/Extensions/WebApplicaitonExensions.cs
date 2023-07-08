﻿using Microsoft.EntityFrameworkCore;

namespace QuickShop.Api.Extensions;

public static class WebApplicaitonExensions 
{
    public static async Task MigrateDatabaseAsync<TDbContext>(this WebApplication app)
        where TDbContext : DbContext
    {
        var services = app.Services;

        using (var scope = services.CreateAsyncScope())
        {
            var serviceProvider = scope.ServiceProvider;

            var dbContext = serviceProvider.GetRequiredService<TDbContext>();

            await dbContext.Database.MigrateAsync();
        }
    }
}