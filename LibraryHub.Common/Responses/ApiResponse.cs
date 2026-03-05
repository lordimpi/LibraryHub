namespace LibraryHub.Common.Responses;

/// <summary>
/// Representa una respuesta estándar para operaciones de API.
/// </summary>
/// <typeparam name="T">Tipo del dato de respuesta.</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// Obtiene o establece si la operación fue exitosa.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Obtiene o establece el mensaje descriptivo de la respuesta.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece la carga útil asociada a la respuesta.
    /// </summary>
    public T? Data { get; set; }
}
