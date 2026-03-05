namespace LibraryHub.Common.DTOs;

/// <summary>
/// Define los datos requeridos para crear un autor.
/// </summary>
public class CreateAuthorDto
{
    /// <summary>
    /// Obtiene o establece el nombre completo del autor.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece la fecha de nacimiento del autor.
    /// </summary>
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// Obtiene o establece la ciudad del autor.
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece el correo electrónico del autor.
    /// </summary>
    public string Email { get; set; } = string.Empty;
}
