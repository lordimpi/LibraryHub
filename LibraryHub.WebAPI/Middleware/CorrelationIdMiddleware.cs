namespace LibraryHub.WebAPI.Middleware;

/// <summary>
/// Middleware que gestiona el identificador de correlacion por solicitud.
/// </summary>
/// <remarks>
/// Lee el encabezado <c>X-Correlation-Id</c> si existe, o genera uno nuevo.
/// Luego lo expone en request, response y <see cref="HttpContext.Items"/>
/// para trazabilidad en logs.
/// </remarks>
public class CorrelationIdMiddleware
{
    /// <summary>
    /// Nombre del encabezado HTTP usado para correlacion.
    /// </summary>
    public const string CorrelationHeaderName = "X-Correlation-Id";

    private readonly RequestDelegate _next;

    /// <summary>
    /// Inicializa una nueva instancia del middleware.
    /// </summary>
    /// <param name="next">Siguiente delegado del pipeline.</param>
    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Ejecuta la logica del middleware.
    /// </summary>
    /// <param name="context">Contexto HTTP actual.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(CorrelationHeaderName, out var correlationId))
        {
            correlationId = Guid.NewGuid().ToString();
            context.Request.Headers.Append(CorrelationHeaderName, correlationId);
        }

        context.Response.Headers.Append(CorrelationHeaderName, correlationId);
        context.Items[CorrelationHeaderName] = correlationId.ToString();

        await _next(context);
    }
}
