namespace LibraryHub.Common.IOptionPattern;

/// <summary>
/// Define un acceso genérico a configuración mediante opciones estáticas, por solicitud y monitoreadas.
/// </summary>
/// <typeparam name="T">Tipo de opciones de configuración.</typeparam>
public interface IGenericOptionsService<T> where T : class, new()
{
    /// <summary>
    /// Obtiene la configuración actual desde <c>IOptions&lt;T&gt;</c>.
    /// </summary>
    /// <returns>Instancia de configuración de tipo <typeparamref name="T"/>.</returns>
    T GetOptions();

    /// <summary>
    /// Obtiene una instantánea de configuración desde <c>IOptionsSnapshot&lt;T&gt;</c>.
    /// </summary>
    /// <returns>Instancia de configuración de tipo <typeparamref name="T"/> para la solicitud actual.</returns>
    T GetSnapshotOptions();

    /// <summary>
    /// Obtiene la configuración monitoreada desde <c>IOptionsMonitor&lt;T&gt;</c>.
    /// </summary>
    /// <returns>Instancia de configuración actual de tipo <typeparamref name="T"/>.</returns>
    T GetMonitorOptions();
}
