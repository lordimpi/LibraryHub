namespace LibraryHub.Common.Configurations;

/// <summary>
/// Define la configuración del cliente HTTP para consumo de APIs.
/// </summary>
public class ApiClientOptions
{
    /// <summary>
    /// Obtiene o establece la URL base del backend o API objetivo.
    /// </summary>
    public string BaseUrl { get; set; } = string.Empty;
}
