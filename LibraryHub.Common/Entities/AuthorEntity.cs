namespace LibraryHub.Common.Entities;

/// <summary>
/// Representa un autor dentro del dominio de LibraryHub.
/// </summary>
public class AuthorEntity
{
    /// <summary>
    /// Obtiene o establece el identificador único del autor.
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
    /// Obtiene o establece la ciudad de residencia del autor.
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece el correo electrónico del autor.
    /// </summary>
    public string Email { get; set; } = string.Empty;
}
