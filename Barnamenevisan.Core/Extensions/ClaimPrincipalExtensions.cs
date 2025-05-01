using System.Security.Claims;

namespace Barnamenevisan.Core.Extensions;

public static class ClaimPrincipalExtensions
{
    public static string GetUserName(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(ClaimTypes.Name)?.Value;
    }

    public static bool GetIsAdmin(this ClaimsPrincipal principal)
    {
        return Convert.ToBoolean(principal.FindFirst("IsAdmin")?.Value);
    }
}