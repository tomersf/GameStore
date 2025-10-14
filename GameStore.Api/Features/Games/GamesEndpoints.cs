using GameStore.Api.Features.Games.CreateGame;
using GameStore.Api.Features.Games.DeleteGame;
using GameStore.Api.Features.Games.GetGame;
using GameStore.Api.Features.Games.GetGames;
using GameStore.Api.Features.Games.UpdateGame;

namespace GameStore.Api.Features.Games;

public static class GamesEndpoints
{
    public static void MapGames(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/games")
            .WithTags("Games");

        group.MapGetGame();
        group.MapGetGames();
        group.MapCreateGame();
        group.MapUpdateGame();
        group.MapDeleteGame();
    }
}