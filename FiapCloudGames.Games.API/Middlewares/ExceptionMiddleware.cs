using FiapCloudGames.Games.Domain.Exceptions;
using Serilog;

namespace FiapCloudGames.Games.API.Middlewares;

public class ExceptionMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = ex switch
            {
                GameNotFoundException or
                OrderNotFoundException or
                GameGenreNotFoundException => StatusCodes.Status404NotFound,
                InvalidFormException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            if (context.Response.StatusCode == StatusCodes.Status500InternalServerError)
                Log.Error(ex, "Erro interno no serviço FiapCloudGames.Games");
        }
    }
}
