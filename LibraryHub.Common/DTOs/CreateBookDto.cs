using System.ComponentModel.DataAnnotations;

namespace LibraryHub.Common.DTOs;

/// <summary>
/// Define los datos requeridos para crear un libro.
/// </summary>
public class CreateBookDto
{
    /// <summary>
    /// Obtiene o establece el titulo del libro.
    /// </summary>
    [Required]
    [StringLength(250)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece el anio de publicacion.
    /// </summary>
    [Range(1, 3000)]
    public int Year { get; set; }

    /// <summary>
    /// Obtiene o establece el genero literario.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Genre { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece la cantidad de paginas.
    /// </summary>
    [Range(1, 100000)]
    public int Pages { get; set; }

    /// <summary>
    /// Obtiene o establece el identificador del autor asociado.
    /// </summary>
    [Range(1, int.MaxValue)]
    public int AuthorId { get; set; }
}
