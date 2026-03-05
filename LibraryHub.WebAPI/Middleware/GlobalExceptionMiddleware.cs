using LibraryHub.Common.Exceptions;

namespace LibraryHub.WebAPI.Middleware;

/// <summary>
/// Middleware para manejo global de excepciones en la API.
/// </summary>
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    /// <summary>
    /// Inicializa una nueva instancia del middleware.
    /// </summary>
    /// <param name="next">Delegado del siguiente middleware en el pipeline.</param>
    /// <param name="logger">Logger para registrar excepciones y contexto.</param>
    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Ejecuta el middleware capturando excepciones de dominio y excepciones no controladas.
    /// </summary>
    /// <param name="context">Contexto HTTP actual.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Items[CorrelationIdMiddleware.CorrelationHeaderName]?.ToString() ?? "N/A";

        using (_logger.BeginScope(new Dictionary<string, object> { ["CorrelationId"] = correlationId }))
        {
            try
            {
                await _next(context);
            }
            catch (AuthorNotFoundException ex)
            {
                _logger.LogWarning(ex, "Author not found.");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new { message = ex.Message });
            }
            catch (MaxBooksExceededException ex)
            {
                _logger.LogWarning(ex, "Business rule exceeded: max books allowed.");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new { message = ex.Message });
            }
            catch (BookNotFoundException ex)
            {
                _logger.LogWarning(ex, "Book not found.");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception in request pipeline.");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new { message = "Internal Server Error" });
            }
        }
    }
}
