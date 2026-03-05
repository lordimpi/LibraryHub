using LibraryHub.Common.Exceptions;

namespace LibraryHub.WebAPI.Middleware;

/// <summary>
/// Middleware para manejo global de excepciones en la API.
/// </summary>
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Inicializa una nueva instancia del middleware.
    /// </summary>
    /// <param name="next">Delegado del siguiente middleware en la canalización.</param>
    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Ejecuta el middleware capturando excepciones de dominio y excepciones no controladas.
    /// </summary>
    /// <param name="context">Contexto HTTP actual.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (AuthorNotFoundException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new { message = ex.Message });
        }
        catch (MaxBooksExceededException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new { message = ex.Message });
        }
        catch (Exception)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new { message = "Internal Server Error" });
        }
    }
}
