using System.Net;
using System.Text.Json;
using PFMS.API.Models;
using PFMS.Utils.Custom_Exceptions;
using PFMS.Utils.Enums;

namespace PFMS.API.Middlewares
{
    /// <summary>
    /// Exception Handler middleware
    /// </summary>
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Middleware function
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (CustomException ex)
            {
                _logger.LogError(ex, $"Log Id: {Guid.NewGuid()} - {ex.Message}");
                await HandleCustomExceptionAsync(httpContext, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Log Id: {Guid.NewGuid()} - {ex.Message}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        /// <summary>
        /// This method handles the Exception of type Exception class
        /// </summary>
        /// <param name="context">HttpContect object</param>
        /// <param name="exception">Object of Exception class</param>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new ErrorResponseModel
            {
                StatusCode = context.Response.StatusCode,
                ErrorName = ErrorNames.INTERNAL_SERVER_ERROR.ToString(),
                ErrorMessage = exception.Message
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(jsonResponse);
        }

        /// <summary>
        /// This method handles the Custom exceptions defined by the developers
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="exception">Object of CustomException class or its child classes</param>
        private async Task HandleCustomExceptionAsync(HttpContext context, CustomException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception.StatusCode;
            var response = new ErrorResponseModel()
            {
                StatusCode = context.Response.StatusCode,
                ErrorName = exception.Name,
                ErrorMessage = exception.Message
            };
            var jsonResponse = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
