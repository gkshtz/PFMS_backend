using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace PFMS.Utils.Constants
{
    public static class ErrorMessages
    {
        public const string UserNotFound = "User does not exist.";
        public const string UserNotAuthenticated = "Incorrect Password, authentication failed.";
        public const string MisingAuthorizationHeader = "Authorization header not present.";
        public const string InvalidToken = "Token validation failed";
        public const string TransactionNotFound = "Transaction Not Found";
        public const string TokenMalformed = "Token Malformed";
        public const string EmptyToken = "Token is empty";
        public const string EmptyAuthorizationHeader = "Authorization Header is Empty";
        public const string CategoryNotFound = "Category Does Not Exist";
        public const string CannotDeleteCategory = "User Cannot Delete This Category";
        public const string IncorrectOldPassword = "Old Password is Incorrect";
        public const string RefreshTokenIsNotPresnt = "Refresh Token is not present in the cookies";
        public const string InvalidRefreshToken = "Refresh Token is invalid";
        public const string UserIdNotPresentInRefreshToken = "User ID is not present in the refresh token claims";
        public const string ActionNotAllowed = "User does not has permission";
        public const string FieldMustContainsAlphabeticCharacters = "The field must contain only alphabetic characters";
        public const string OutOfAgeRange = "Age must be greater than 15 and less than or equal to 100";
        public const string ShortPassword = "Password length cannot be lesser than 8";
        public const string LongTransactionNameLength = "Length of transaction name must not exceed 100";
        public const string LongTransactionDescriptionLength = "Length of description name must not exceed 100";
        public const string InvalidTransactionType = "Invalid transaction type";
        public const string OtpShouldContainDigits = "One Time Password can only contain digits";
        public const string OtpLengthShouldBeSix = "Length of One Time Password should be six only";
        public const string OtpDoesNotFound = "Otp does not exist";
        public const string UniqueDeviceIdNotMatch = "Cannot verify otp with this device.";
        public const string UniqueDeviceIdNotPresentInCookies = "Unique Device Id is not present in the cookies";
        public const string UserNotAllowedToResetPassword = "User cannot reset other user's password.";
        public const string OtpIsExpired = "OTP is Expired.";
        public const string OtpNotVerified = "OTP is not verified yet.";
        public const string InvalidMonth = "Month is invalid.";
        public const string InvalidYear = "Year is invalid.";
        public const string BudgetAmountRageExceeded = "Budget amount is out of range.";
        public const string InvalidMonthOrYear = "Invalid month or year.";
        public const string BudgetNotFound = "Budget Not Found.";
        public const string BudgetMonthCannotBeOfFuture = "Budget month cannot be of future.";
        public const string TransactionCanOnlyBeSetOfPresentMonth = "Transaction can only be set for present month.";
        public const string OtpAlreadyVerified = "OTP was already verified, but now becomes expired.";
    }
}
