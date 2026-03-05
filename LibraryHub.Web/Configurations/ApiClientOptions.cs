namespace LibraryHub.Web.Configurations;

/// <summary>
/// Define la configuración del cliente HTTP para consumo de la API.
/// </summary>
public class ApiClientOptions
{
    /// <summary>
    /// Obtiene o establece la URL base del backend.
    /// </summary>
    public string BaseUrl { get; set; } = string.Empty;
}
