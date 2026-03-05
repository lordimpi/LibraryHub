namespace LibraryHub.Common.Entities;

/// <summary>
/// Representa un libro dentro del dominio de LibraryHub.
/// </summary>
public class BookEntity
{
    /// <summary>
    /// Obtiene o establece el identificador unico del libro.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Obtiene o establece el titulo del libro.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece el anio de publicacion.
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// Obtiene o establece el genero literario.
    /// </summary>
    public string Genre { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece la cantidad de paginas del libro.
    /// </summary>
    public int Pages { get; set; }

    /// <summary>
    /// Obtiene o establece el identificador del autor asociado.
    /// </summary>
    public int AuthorId { get; set; }

    /// <summary>
    /// Obtiene o establece la navegacion al autor del libro.
    /// </summary>
    public AuthorEntity? Author { get; set; }

    /// <summary>
    /// Obtiene o establece un valor que indica si el libro fue eliminado logicamente.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Obtiene o establece la fecha UTC en la que el libro fue eliminado logicamente.
    /// </summary>
    public DateTime? DeletedAtUtc { get; set; }
}
