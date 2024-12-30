using System.Diagnostics;
using System.Numerics;
using System.Runtime.Serialization;
using PFMS.Utils.Enums;

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

        public const string BudgetMailSubject = "Budget Alert";

        public const string BudgetExceededMailSubject = "Your Budget Exceeded!";

        public const string BudgetUpdateMailSubject = "Budget Updated!";

        public static string GenerateBudgetUpdateEmailSubject(string username, Months month, int year)
        {
            string body;
            body = $"<p>Hi {username},</p>\r\n<div>Your Budget for {char.ToUpper(month.ToString()[0]) + month.ToString().Substring(1).ToLower()}, {year} has been updated!</div>\r\n<div>Thanks.</div>";
            return body;
        }

        public static string GenerateBudgetSetEmailBody(string userName, decimal budgetAmount, decimal spentPercentage, Months month, int year)
        { 
            string body = $"<p>Hi {userName},</p>\r\n<div>You have set budget of RS {budgetAmount} for {char.ToUpper(month.ToString()[0]) + month.ToString().Substring(1).ToLower()}, {year}. You have already spend {spentPercentage}% of total set budget.</div>\r\n<div>Thanks</div>";
            return body;
        }
        public static string GenerateOtpEmailBody(string otp, string firstName, int validityInMinutes)
        {
            string body = $"<p>Hi {firstName},<p>\r\n<div>One Time Password for resetting your password is {otp}. OTP is valid for {validityInMinutes} minutes<div>\r\n<div>Thanks</div>";
            return body;
        }

        public static string GenerateBudgetExceededMailBody(string userName, Months month, int year, decimal exceededAmount)
        {
            string body;
            body = $"<p>Hi {userName}</p>\r\n  <div>Your expenditure for {char.ToUpper(month.ToString()[0]) + month.ToString().Substring(1).ToLower()}, {year} has exceeded your budget by {exceededAmount}.</div>\r\n<div>Thanks</div>";
            return body;
        }
    }
}
