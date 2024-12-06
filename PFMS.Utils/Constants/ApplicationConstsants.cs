using System.Numerics;

namespace PFMS.Utils.Constants
{
    public static class ApplicationConstsants
    {
        public static readonly List<string> BypassRoutes = new List<string>()
        {
            "/api/users/login",
            "/api/users/refreshed-access-token",
            "/api/users/logout"
        };

        public const string RefreshToken = "refresh-token";
    }
}
