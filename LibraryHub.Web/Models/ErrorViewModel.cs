namespace LibraryHub.Web.Models;

/// <summary>
/// Representa el modelo de datos para la vista de error.
/// </summary>
public class ErrorViewModel
{
    /// <summary>
    /// Obtiene o establece el identificador de la solicitud.
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// Obtiene un valor que indica si el identificador de solicitud debe mostrarse.
    /// </summary>
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
