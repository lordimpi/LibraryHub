using LibraryHub.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

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
                await WriteProblemDetailsAsync(
                    context,
                    StatusCodes.Status400BadRequest,
                    ex.Message,
                    ex.Message,
                    "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1",
                    correlationId);
            }
            catch (MaxBooksExceededException ex)
            {
                _logger.LogWarning(ex, "Business rule exceeded: max books allowed.");
                await WriteProblemDetailsAsync(
                    context,
                    StatusCodes.Status400BadRequest,
                    ex.Message,
                    ex.Message,
                    "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1",
                    correlationId);
            }
            catch (BookNotFoundException ex)
            {
                _logger.LogWarning(ex, "Book not found.");
                await WriteProblemDetailsAsync(
                    context,
                    StatusCodes.Status400BadRequest,
                    ex.Message,
                    ex.Message,
                    "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1",
                    correlationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception in request pipeline.");
                await WriteProblemDetailsAsync(
                    context,
                    StatusCodes.Status500InternalServerError,
                    "Internal Server Error",
                    "Internal Server Error",
                    "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.1",
                    correlationId);
            }
        }
    }

    /// <summary>
    /// Escribe una respuesta estandarizada en formato ProblemDetails.
    /// </summary>
    /// <param name="context">Contexto HTTP actual.</param>
    /// <param name="statusCode">Codigo de estado HTTP.</param>
    /// <param name="title">Titulo del problema.</param>
    /// <param name="detail">Detalle del problema.</param>
    /// <param name="type">URI del tipo de problema.</param>
    /// <param name="correlationId">Identificador de correlacion de la solicitud.</param>
    private static Task WriteProblemDetailsAsync(
        HttpContext context,
        int statusCode,
        string title,
        string detail,
        string type,
        string correlationId)
    {
        context.Response.StatusCode = statusCode;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Type = type,
            Instance = context.Request.Path,
        };

        problemDetails.Extensions["traceId"] = context.TraceIdentifier;
        problemDetails.Extensions["correlationId"] = correlationId;

        return context.Response.WriteAsJsonAsync(problemDetails);
    }
}
