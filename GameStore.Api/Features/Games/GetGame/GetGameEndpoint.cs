using GameStore.Api.Data;
using GameStore.Api.Features.Games.Constants;
using GameStore.Api.Shared.Cdn;

namespace GameStore.Api.Features.Games.GetGame;

public static class GetGameEndpoint
{
    public static void MapGetGame(this IEndpointRouteBuilder app)
    {
        app.MapGet("/{id}",
            async (Guid id, GameStoreContext dbContext,
                CdnUrlTransformer cdnUrlTransformer) =>
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
                        cdnUrlTransformer.TransformToCdnUrl(game.ImageUri),
                        game.LastUpdatedBy))
                    : Results.NotFound();
            }).WithName(EndpointNames.GetGame).AllowAnonymous();
    }
}