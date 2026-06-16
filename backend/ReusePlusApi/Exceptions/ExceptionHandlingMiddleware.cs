using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ReusePlusApi.DTOs;

namespace ReusePlusApi.Exceptions
{
    /// <summary>
    /// Middleware para tratamento centralizado de exceções
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new ErrorResponseDto();

            switch (exception)
            {
                case KeyNotFoundException:
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    response.Message = exception.Message;
                    response.ErrorCode = "NOT_FOUND";
                    break;

                case UnauthorizedAccessException:
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    response.Message = exception.Message;
                    response.ErrorCode = "UNAUTHORIZED";
                    break;

                case InvalidOperationException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response.Message = exception.Message;
                    response.ErrorCode = "BAD_REQUEST";
                    break;

                case ArgumentException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response.Message = exception.Message;
                    response.ErrorCode = "INVALID_ARGUMENT";
                    break;

                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.Message = "An unexpected error occurred. Please try again later.";
                    response.ErrorCode = "INTERNAL_SERVER_ERROR";
                    break;
            }

            var jsonSerializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, jsonSerializerOptions);

            return context.Response.WriteAsync(json);
        }
    }
}
