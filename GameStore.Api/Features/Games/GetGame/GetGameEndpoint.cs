using GameStore.Api.Data;
using GameStore.Api.Features.Games.Constants;

namespace GameStore.Api.Features.Games.GetGame;

public static class GetGameEndpoint
{
    
    public static void MapGetGame(this IEndpointRouteBuilder app)
    {
        app.MapGet("/{id}", (Guid id, GameStoreData data) =>
        {
            var game = data.GetGame(id);
            return game is not null
                ? Results.Ok(new GameDetailsDto(
                    game.Id,
                    game.Name,
                    game.GenreId,
                    game.Price,
                    game.ReleaseDate,
                    game.Description))
                : Results.NotFound();
        }).WithName(EndpointNames.GetGame);
    }
    
}