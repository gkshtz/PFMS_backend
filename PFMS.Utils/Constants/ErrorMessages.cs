﻿namespace PFMS.Utils.Constants
{
    public static class ErrorMessages
    {
        public const string UserNotFound = "User with this email does not exist.";
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
        public const string OutOfAgeRange = "Age must be greater than 15 and less than or equal to 100.";
        public const string ShortPassword = "Password length cannot be lesser than 8";
    }
}
