using System.Security.Claims;

namespace PrimeCare.Api.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string RetrieveEmailFromPricipal(this ClaimsPrincipal user)
        {
            return user?.Claims?.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
        }

    }
}
