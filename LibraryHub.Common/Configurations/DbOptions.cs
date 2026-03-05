namespace LibraryHub.Common.Configurations;

/// <summary>
/// Representa la configuración de conexión a base de datos.
/// </summary>
public class DbOptions
{
    /// <summary>
    /// Obtiene o establece la cadena de conexión principal de la aplicación.
    /// </summary>
    public string DefaultConnection { get; set; } = string.Empty;
}
