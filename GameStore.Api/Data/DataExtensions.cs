using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public static class DataExtensions
{
    public static async Task InitializeDbAsync(this WebApplication app)
    {
        await app.MigrateDbAsync();
        await app.SeedDbAsync();
        app.Logger.LogInformation(18, "Database initialized!");
    }

    private static async Task MigrateDbAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
        await db.Database.MigrateAsync();
    }

    private static async Task SeedDbAsync(this WebApplication app)
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
        await db.SaveChangesAsync();
    }
}