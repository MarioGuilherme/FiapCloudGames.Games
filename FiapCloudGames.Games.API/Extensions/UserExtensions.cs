using System.Security.Claims;

namespace FiapCloudGames.Games.API.Extensions;

public static class UserExtensions
{
    public static int UserId(this ClaimsPrincipal principal) => int.Parse(principal.Claims.First(c => c.Type == "UserId").Value);
}