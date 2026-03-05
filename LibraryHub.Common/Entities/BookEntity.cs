namespace LibraryHub.Common.Entities;

/// <summary>
/// Representa un libro dentro del dominio de LibraryHub.
/// </summary>
public class BookEntity
{
    /// <summary>
    /// Obtiene o establece el identificador único del libro.
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
    /// Obtiene o establece la cantidad de páginas del libro.
    /// </summary>
    public int Pages { get; set; }

    /// <summary>
    /// Obtiene o establece el identificador del autor asociado.
    /// </summary>
    public int AuthorId { get; set; }

    /// <summary>
    /// Obtiene o establece la navegación al autor del libro.
    /// </summary>
    public AuthorEntity? Author { get; set; }
}
