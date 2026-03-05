namespace LibraryHub.Common.DTOs;

/// <summary>
/// Representa la información de salida de un autor.
/// </summary>
public class AuthorDto
{
    /// <summary>
    /// Obtiene o establece el identificador del autor.
    /// </summary>
    public int Id { get; set; }

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
