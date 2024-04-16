using System.Security.Claims;

namespace Common
{
    public static class Utils
    {
        public static string GetUserId(ClaimsPrincipal User)
        {
            // Check if the user is authenticated
            if (User?.Identity?.IsAuthenticated == true)
            {
                // Retrieve the user ID from the claims
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    // Return the user ID
                    return userIdClaim.Value;
                }
            }
            return null;
        }
    }
}
