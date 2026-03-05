using LibraryHub.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LibraryHub.WebAPI.Filters;

/// <summary>
/// Captura excepciones de dominio y las traduce a respuestas HTTP controladas.
/// </summary>
public sealed class DomainExceptionFilter : IExceptionFilter
{
    private readonly ILogger<DomainExceptionFilter> _logger;

    /// <summary>
    /// Inicializa una nueva instancia del filtro de excepciones de dominio.
    /// </summary>
    /// <param name="logger">Logger para registrar los eventos de excepcion controlada.</param>
    public DomainExceptionFilter(ILogger<DomainExceptionFilter> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Ejecuta el manejo de excepciones al producirse una falla en la accion MVC.
    /// </summary>
    /// <param name="context">Contexto de excepcion de MVC.</param>
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is AuthorNotFoundException
            or MaxBooksExceededException
            or BookNotFoundException)
        {
            _logger.LogWarning(context.Exception, "Domain exception handled by MVC filter.");
            context.Result = new ObjectResult(CreateProblemDetails(context))
            {
                StatusCode = StatusCodes.Status400BadRequest,
            };
            context.ExceptionHandled = true;
        }
    }

    /// <summary>
    /// Construye un objeto <see cref="ProblemDetails"/> para excepciones de dominio.
    /// </summary>
    /// <param name="context">Contexto de excepcion de MVC.</param>
    /// <returns>Instancia estandarizada de problema HTTP.</returns>
    private static ProblemDetails CreateProblemDetails(ExceptionContext context)
    {
        var problemDetails = new ProblemDetails
        {
            Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1",
            Title = context.Exception.Message,
            Detail = context.Exception.Message,
            Status = StatusCodes.Status400BadRequest,
            Instance = context.HttpContext.Request.Path,
        };

        problemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
        return problemDetails;
    }
}
