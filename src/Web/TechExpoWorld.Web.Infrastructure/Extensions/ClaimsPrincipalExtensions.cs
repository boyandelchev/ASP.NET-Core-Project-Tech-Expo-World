namespace TechExpoWorld.Web.Infrastructure.Extensions
{
    using System.Security.Claims;

    using static TechExpoWorld.Common.GlobalConstants.Admin;

    public static class ClaimsPrincipalExtensions
    {
        public static string Id(this ClaimsPrincipal user)
            => user.FindFirst(ClaimTypes.NameIdentifier).Value;

        public static bool IsAdmin(this ClaimsPrincipal user)
            => user.IsInRole(RoleName);
    }
}
