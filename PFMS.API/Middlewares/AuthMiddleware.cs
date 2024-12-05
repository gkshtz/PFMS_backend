using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PFMS.Utils.Constants;
using PFMS.Utils.CustomExceptions;
using PFMS.Utils.Enums;

namespace PFMS.API.Middlewares
{
    public class AuthMiddleware
    {
        private readonly IConfiguration _configuration;
        private readonly RequestDelegate _next;
        public AuthMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if(CheckBypassRoute(context.Request.Path.Value!))
            {
                await _next(context);
                return;
            }

            var headers = context.Request.Headers;
            if(headers.ContainsKey("Authorization")==false)
            {
                throw new UnauthorizedException(ErrorMessages.MisingAuthorizationHeader);
            }

            var authorizationContent = headers["Authorization"].FirstOrDefault();
            if(authorizationContent == null)
            {
                throw new UnauthorizedException(ErrorMessages.EmptyAuthorizationHeader);
            }
            List<string> tokenParts = authorizationContent.ToString().Split(" ").ToList();

            if(tokenParts.Count != 2)
            {
                throw new UnauthorizedAccessException(ErrorMessages.TokenMalformed);
            }
            string type = tokenParts.First();
            if(type != "Bearer")
            {
                throw new UnauthorizedException(ErrorMessages.TokenMalformed);
            }

            string token = tokenParts.Last();
            ClaimsPrincipal? principal = ValidateToken(token);

            if(principal == null)
            {
                throw new UnauthorizedException(ErrorMessages.InvalidToken);
            }

            // Attach the user (ClaimsPrincipal) to the HttpContext
            context.User = principal;

            await _next(context);
        }

        private ClaimsPrincipal? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["jwt:Key"]!);

            try
            {
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return principal;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private bool CheckBypassRoute(string path)
        {
            var res = ApplicationConstsants.BypassRoutes.Contains(path);
            return res;
        }

    }
}
