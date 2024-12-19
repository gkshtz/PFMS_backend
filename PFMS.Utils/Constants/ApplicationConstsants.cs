using System.Diagnostics;
using System.Numerics;
using System.Runtime.Serialization;

namespace PFMS.Utils.Constants
{
    public static class ApplicationConstsants
    {
        public static readonly List<string> BypassRoutes = new List<string>()
        {
            "/api/users/login",
            "/api/users/refresh-token/refresh",
            "/api/users/logout",
            "/api/users/otp/send",
            "/api/users/otp/verify",
            "/api/users/otp/reset-password"
        };

        public const string RefreshToken = "refresh-token";

        public const string UniqueDeviceId = "unique-device-id";

        public const string OtpEmailSubject = "Reset Your Password";

        public static string GenerateOtpEmailBody(string otp, string firstName, int validityInMinutes)
        {
            string body = $"<p>Hi {firstName},<p>\r\n<div>One Time Password for resetting your password is {otp}. OTP is valid for {validityInMinutes} minutes<div>\r\n<div>Thanks</div>";
            return body;
        }
    }
}
