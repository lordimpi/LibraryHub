using Microsoft.Extensions.Options;

namespace LibraryHub.Common.IOptionPattern;

/// <summary>
/// Implementa acceso genérico a opciones de configuración usando los contratos de opciones de .NET.
/// </summary>
/// <typeparam name="T">Tipo de opciones de configuración.</typeparam>
public class GenericOptionsService<T> : IGenericOptionsService<T> where T : class, new()
{
    private readonly IOptions<T> _options;
    private readonly IOptionsSnapshot<T> _snapshotOptions;
    private readonly IOptionsMonitor<T> _monitorOptions;

    /// <summary>
    /// Inicializa una nueva instancia del servicio genérico de opciones.
    /// </summary>
    /// <param name="options">Proveedor de opciones estáticas.</param>
    /// <param name="snapshotOptions">Proveedor de opciones por solicitud.</param>
    /// <param name="monitorOptions">Proveedor de opciones monitoreadas.</param>
    public GenericOptionsService(
        IOptions<T> options,
        IOptionsSnapshot<T> snapshotOptions,
        IOptionsMonitor<T> monitorOptions)
    {
        _options = options;
        _snapshotOptions = snapshotOptions;
        _monitorOptions = monitorOptions;
    }

    /// <inheritdoc />
    public T GetOptions()
    {
        return _options.Value;
    }

    /// <inheritdoc />
    public T GetSnapshotOptions()
    {
        return _snapshotOptions.Value;
    }

    /// <inheritdoc />
    public T GetMonitorOptions()
    {
        return _monitorOptions.CurrentValue;
    }
}
