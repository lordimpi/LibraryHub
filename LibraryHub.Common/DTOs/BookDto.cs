namespace LibraryHub.Common.DTOs;

/// <summary>
/// Representa la información de salida de un libro.
/// </summary>
public class BookDto
{
    /// <summary>
    /// Obtiene o establece el identificador del libro.
    /// </summary>
    public int Id { get; set; }

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

    /// <summary>
    /// Obtiene o establece el nombre completo del autor.
    /// </summary>
    public string AuthorFullName { get; set; } = string.Empty;
}
