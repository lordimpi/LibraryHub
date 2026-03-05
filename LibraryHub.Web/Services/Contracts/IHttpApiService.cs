namespace LibraryHub.Web.Services.Contracts;

/// <summary>
/// Define operaciones HTTP genéricas para consumo del backend desde MVC.
/// </summary>
public interface IHttpApiService
{
    /// <summary>
    /// Ejecuta una petición GET y deserializa la respuesta.
    /// </summary>
    /// <typeparam name="TResponse">Tipo de respuesta esperada.</typeparam>
    /// <param name="url">Ruta relativa o absoluta del recurso.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Respuesta deserializada.</returns>
    Task<TResponse?> GetAsync<TResponse>(string url, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ejecuta una petición POST y deserializa la respuesta.
    /// </summary>
    /// <typeparam name="TRequest">Tipo del cuerpo de la solicitud.</typeparam>
    /// <typeparam name="TResponse">Tipo de respuesta esperada.</typeparam>
    /// <param name="url">Ruta relativa o absoluta del recurso.</param>
    /// <param name="payload">Cuerpo de la solicitud.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Respuesta deserializada.</returns>
    Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest payload, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ejecuta una petición PUT y deserializa la respuesta.
    /// </summary>
    /// <typeparam name="TRequest">Tipo del cuerpo de la solicitud.</typeparam>
    /// <typeparam name="TResponse">Tipo de respuesta esperada.</typeparam>
    /// <param name="url">Ruta relativa o absoluta del recurso.</param>
    /// <param name="payload">Cuerpo de la solicitud.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Respuesta deserializada.</returns>
    Task<TResponse?> PutAsync<TRequest, TResponse>(string url, TRequest payload, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ejecuta una petición PATCH y deserializa la respuesta.
    /// </summary>
    /// <typeparam name="TRequest">Tipo del cuerpo de la solicitud.</typeparam>
    /// <typeparam name="TResponse">Tipo de respuesta esperada.</typeparam>
    /// <param name="url">Ruta relativa o absoluta del recurso.</param>
    /// <param name="payload">Cuerpo de la solicitud.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Respuesta deserializada.</returns>
    Task<TResponse?> PatchAsync<TRequest, TResponse>(string url, TRequest payload, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ejecuta una petición DELETE y deserializa la respuesta.
    /// </summary>
    /// <typeparam name="TResponse">Tipo de respuesta esperada.</typeparam>
    /// <param name="url">Ruta relativa o absoluta del recurso.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Respuesta deserializada.</returns>
    Task<TResponse?> DeleteAsync<TResponse>(string url, CancellationToken cancellationToken = default);
}
