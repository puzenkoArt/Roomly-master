﻿using Microsoft.EntityFrameworkCore;
using Roomly.Shared.Data;

namespace Roomly.Users.Infrastructure.Extensions;

public static class MigrationExtension
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Database.Migrate();
        
        DbInitializer.Initialize(context);
    }
}