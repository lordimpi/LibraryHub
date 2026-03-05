namespace LibraryHub.Common.DTOs;

/// <summary>
/// Define los datos requeridos para crear un libro.
/// </summary>
public class CreateBookDto
{
    /// <summary>
    /// Obtiene o establece el título del libro.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece el año de publicación.
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// Obtiene o establece el género literario.
    /// </summary>
    public string Genre { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece la cantidad de páginas.
    /// </summary>
    public int Pages { get; set; }

    /// <summary>
    /// Obtiene o establece el identificador del autor asociado.
    /// </summary>
    public int AuthorId { get; set; }
}
