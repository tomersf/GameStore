using System.Security.Claims;

namespace GameStore.Api.Shared.Authorization;

public static class ClaimsExtensions
{
    public static void TransformScopeClaim(this ClaimsIdentity? identity,
        string sourceScopeClaimType)
    {
        var scopeClaim = identity?.FindFirst(sourceScopeClaimType);

        if (scopeClaim == null) return;

        var scopes =
            scopeClaim.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        identity?.RemoveClaim(scopeClaim);

        identity?.AddClaims(
            scopes.Select(scope => new Claim(GameStoreClaimTypes.Scope, scope)));
    }

    public static void LogAllClaims(this ClaimsPrincipal? principal, ILogger logger)
    {
        var claims = principal?.Claims;
        if (claims == null) return;
        
        foreach (var claim in claims)
        {
            logger.LogTrace(
                "Claim: {ClaimType}, Value: {ClaimValue}", claim.Type,
                claim.Value);
        }
    }
    
    public static void MapUserIdClaim(this ClaimsIdentity? identity,
        string sourceClaimType)
    {
        var sourceClaim = identity?.FindFirst(sourceClaimType);

        if (sourceClaim == null) return;

        identity?.AddClaim(new Claim(GameStoreClaimTypes.UserId, sourceClaim.Value));
    }
}