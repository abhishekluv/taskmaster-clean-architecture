using System.Net;
using System.Text.Json;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;

using Serilog;

namespace TaskMaster.WebApi.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unhandled exception while processing request {Path}", context.Request.Path);

                if (context.Response.HasStarted)
                {
                    throw;
                }

                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var traceId = context.TraceIdentifier;

            var problemDetails = new ProblemDetails
            {
                Instance = context.Request.Path,
                Extensions = { ["traceId"] = traceId }
            };

            switch (exception)
            {
                case ValidationException validationEx:
                    problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                    problemDetails.Title = "One or more validation errors occurred.";
                    problemDetails.Status = (int)HttpStatusCode.BadRequest;
                    problemDetails.Detail = "See the errors property for details.";
                    problemDetails.Extensions["errors"] = validationEx.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray());

                    context.Response.StatusCode = problemDetails.Status.Value;
                    break;
                case KeyNotFoundException:
                    problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4";
                    problemDetails.Title = "The specified resource was not found.";
                    problemDetails.Status = (int)HttpStatusCode.NotFound;
                    problemDetails.Detail = exception.Message;
                    context.Response.StatusCode = problemDetails.Status.Value;
                    break;
                default:
                    problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
                    problemDetails.Title = "An unexpected error occurred.";
                    problemDetails.Status = (int)HttpStatusCode.InternalServerError;
                    problemDetails.Detail = "An internal server error has occurred.";
                    context.Response.StatusCode = problemDetails.Status.Value;
                    break;
            }

            context.Response.ContentType = "application/problem+json";
            var json = JsonSerializer.Serialize(problemDetails, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
    }
}
