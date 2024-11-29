using System.Numerics;

namespace PFMS.Utils.Constants
{
    public static class ApplicationConstsants
    {
        public static readonly List<string> BypassRoutes = new List<string>()
        {
            "/api/users/login",
            "/api/users",
            "/api/users/refreshed-access-token"
        };

        public const string RefreshToken = "refresh-token";
    }
}
