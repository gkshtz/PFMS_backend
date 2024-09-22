using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PFMS.Utils.Constants;
using PFMS.Utils.Custom_Exceptions;
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
                throw new UnauthorizedException();
            }
            string type = authorizationContent.Split(" ").First();
            if(type != "Bearer")
            {
                throw new UnauthorizedException();
            }
            string token = authorizationContent.Split(" ").Last();

            if(string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedException();
            }

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
            var key = Encoding.ASCII.GetBytes(_configuration["jwt:Key"]!);

            try
            {
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
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
            var res = ApplicationConstsants.BypassRoutes.Any(route => path.StartsWith(route));
            return res;
        }

    }
}
