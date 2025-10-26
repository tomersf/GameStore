using System.Security.Claims;
using GameStore.Api.Data;
using GameStore.Api.Features.Games.Constants;
using GameStore.Api.Shared.Authorization;
using GameStore.Api.Shared.FileUpload;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace GameStore.Api.Features.Games.UpdateGame;

public static class UpdateGameEndpoint
{
    public static void MapUpdateGame(this IEndpointRouteBuilder app)
    {
        app.MapPut("/{id}",
                async (Guid id, [FromForm] UpdateGameDto gameDto,
                    GameStoreContext dbContext, FileUploader fileUploader,
                    ClaimsPrincipal user) =>
                {
                    var currentUser =
                        user.FindFirstValue(JwtRegisteredClaimNames.Email) ??
                        user.FindFirstValue(GameStoreClaimTypes.UserId);

                    if (string.IsNullOrEmpty(currentUser))
                    {
                        return Results.Unauthorized();
                    }

                    var existingGame = await dbContext.Games.FindAsync(id);
                    if (existingGame is null)
                    {
                        return Results.NotFound();
                    }

                    if (gameDto.ImageFile is not null)
                    {
                        var fileUploadResult =
                            await fileUploader.UploadFileAsync(
                                gameDto.ImageFile,
                                StorageNames.GameImagesFolder);

                        if (!fileUploadResult.IsSuccess)
                        {
                            return Results.BadRequest(new
                                { message = fileUploadResult.ErrorMessage });
                        }

                        existingGame.ImageUri = fileUploadResult.FileUrl!;
                    }

                    existingGame.Name = gameDto.Name;
                    existingGame.GenreId = gameDto.GenreId;
                    existingGame.Price = gameDto.Price;
                    existingGame.ReleaseDate = gameDto.ReleaseDate;
                    existingGame.Description = gameDto.Description;
                    existingGame.LastUpdatedBy = currentUser;

                    await dbContext.SaveChangesAsync();

                    return Results.NoContent();
                }).WithParameterValidation().DisableAntiforgery()
            .RequireAuthorization(Policies.AdminAccess);
    }
}