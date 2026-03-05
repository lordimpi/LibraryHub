using LibraryHub.WebAPI.Middleware;
using Serilog.Core;
using Serilog.Events;

namespace LibraryHub.WebAPI.Logging;

/// <summary>
/// Enriquecedor de Serilog que agrega el CorrelationId al evento de log.
/// </summary>
public class CorrelationIdEnricher : ILogEventEnricher
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Inicializa una nueva instancia del enriquecedor.
    /// </summary>
    /// <param name="httpContextAccessor">Accesor al contexto HTTP actual.</param>
    public CorrelationIdEnricher(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Agrega la propiedad <c>CorrelationId</c> al evento de log.
    /// </summary>
    /// <param name="logEvent">Evento en construccion.</param>
    /// <param name="propertyFactory">Fabrica de propiedades de log.</param>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var correlationId = _httpContextAccessor.HttpContext?.Items[CorrelationIdMiddleware.CorrelationHeaderName]?.ToString()
                            ?? "N/A";

        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("CorrelationId", correlationId));
    }
}
