using System.Net;
using System.Text.Json;
using LibraryApp.Api.Models;

namespace LibraryApp.Api.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    public ErrorHandlingMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (InvalidOperationException ex)
        {
            await WriteErrorAsync(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            await WriteErrorAsync(context, HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    private static async Task WriteErrorAsync(HttpContext ctx, HttpStatusCode code, string message)
    {
        ctx.Response.StatusCode = (int)code;
        ctx.Response.ContentType = "application/json";
        var resp = ApiResponse<object>.Fail(message);
        await ctx.Response.WriteAsync(JsonSerializer.Serialize(resp));
    }
}