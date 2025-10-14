using System.Security.Claims;
using GameStore.Api.Models;
using GameStore.Api.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;

namespace GameStore.Api.Features.Baskets.Authorization;

public class BasketAuthorizationHandler : AuthorizationHandler<
    OwnerOrAdminRequirement, CustomerBasket>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        OwnerOrAdminRequirement requirement, CustomerBasket resource)
    {
        var currentUserId =
            context.User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (string.IsNullOrEmpty(currentUserId))
        {
            return Task.CompletedTask;
        }

        if (Guid.Parse(currentUserId) == resource.Id ||
            context.User.IsInRole(Roles.Admin))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

public class OwnerOrAdminRequirement : IAuthorizationRequirement
{
}