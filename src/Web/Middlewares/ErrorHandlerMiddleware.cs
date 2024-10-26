using Domain.Exceptions;
using ServiceStack;
using System.Net;
using System;
using System.Text.Json;

namespace Web.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);

                // Status code 401 (No autorizado)
                if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    var response = new
                    {
                        statusCode = context.Response.StatusCode,
                        message = "You are not authorized to access this resource. Please log in."
                    };
                    context.Response.ContentType = "application/json";
                    var result = System.Text.Json.JsonSerializer.Serialize(response);
                    await context.Response.WriteAsync(result);
                }

                // Status code 403 (Prohibido)
                else if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
                {
                    var response = new
                    {
                        statusCode = context.Response.StatusCode,
                        message = "Access to this resource is forbidden."
                    };
                    context.Response.ContentType = "application/json";
                    var result = System.Text.Json.JsonSerializer.Serialize(response);
                    await context.Response.WriteAsync(result);
                }
            }
            catch (Exception ex)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                object errorResponse;
                switch (ex)
                {
                    case UnauthorizedException unauthorizedException:
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        errorResponse = new { statusCode = response.StatusCode, message = unauthorizedException.Message };
                        break;
                    case BadRequestException badRequestException:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        errorResponse = new { statusCode = response.StatusCode, message = badRequestException.Message };
                        break;
                    case NotFoundException notFoundException:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        errorResponse = new { statusCode = response.StatusCode, message = notFoundException.Message };
                        break;
                    case NotAllowedException notAllowedException:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        errorResponse = new { statusCode = response.StatusCode, message = notAllowedException.Message };
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        errorResponse = new { statusCode = response.StatusCode, message = "An unexpected error occurred." };
                        break;
                }

                var result = System.Text.Json.JsonSerializer.Serialize(errorResponse);
                await response.WriteAsync(result);
            }
        }
    }


    public class ErrorResponse
    {
        public string Message { get; set; }
        public string Details { get; set; }

    }
}
