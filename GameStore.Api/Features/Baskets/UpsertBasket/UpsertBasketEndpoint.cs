using GameStore.Api.Data;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Features.Baskets.UpsertBasket;

public static class UpsertBasketEndpoint
{
    public static void MapUpsertBasket(this IEndpointRouteBuilder app)
    {
        app.MapPut("/{userId}", async (Guid userId,
            UpsertBasketDto upsertBasketDto,
            GameStoreContext dbContext) =>
        {
            var basket = await dbContext.Baskets
                .Include(basket => basket.Items)
                .FirstOrDefaultAsync(b => b.Id == userId);

            if (basket is null)
            {
                basket = new CustomerBasket
                {
                    Id = userId,
                    Items = upsertBasketDto.Items.Select(item => new BasketItem
                    {
                        GameId = item.Id,
                        Quantity = item.Quantity
                    }).ToList()
                };

                dbContext.Baskets.Add(basket);
            }
            else
            {
                basket.Items = upsertBasketDto.Items.Select(item =>
                    new BasketItem
                    {
                        GameId = item.Id,
                        Quantity = item.Quantity
                    }).ToList();
            }

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}