﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFMS.Utils.Constants
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
    }
}
