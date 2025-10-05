using System.Diagnostics;
using GameStore.Api.Data;
using GameStore.Api.Features.Games.Constants;
using GameStore.Api.Models;
using Microsoft.Data.Sqlite;

namespace GameStore.Api.Features.Games.GetGame;

public static class GetGameEndpoint
{
    public static void MapGetGame(this IEndpointRouteBuilder app)
    {
        app.MapGet("/{id}",
            async (Guid id, GameStoreContext dbContext) =>
            {
                var game = await dbContext.Games.FindAsync(id);

                return game is not null
                    ? Results.Ok(new GameDetailsDto(
                        game.Id,
                        game.Name,
                        game.GenreId,
                        game.Price,
                        game.ReleaseDate,
                        game.Description,
                        game.ImageUri))
                    : Results.NotFound();
            }).WithName(EndpointNames.GetGame);
    }
}