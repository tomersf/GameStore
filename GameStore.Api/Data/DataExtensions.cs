using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public static class DataExtensions
{
    public static void InitializeDb(this WebApplication app)
    {
        app.MigrateDb();
        app.SeedDb();
    }
    
    private static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
        db.Database.Migrate();
    }

    private static void SeedDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
        if (db.Genres.Any()) return;
        
        db.Genres.AddRange(
            new Genre
            {
                Name = "Action"
            },
            new Genre
            {
                Name = "Adventure"
            },
            new Genre { Name = "RPG" },
            new Genre
            {
                Name = "Strategy"
            });
        db.SaveChanges();
    }
}