using System.ComponentModel.DataAnnotations;

namespace LibraryHub.Common.DTOs;

/// <summary>
/// Define los datos requeridos para actualizar un autor.
/// </summary>
public class UpdateAuthorDto
{
    /// <summary>
    /// Obtiene o establece el nombre completo del autor.
    /// </summary>
    [Required]
    [StringLength(200)]
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece la fecha de nacimiento del autor.
    /// </summary>
    [Required]
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// Obtiene o establece la ciudad del autor.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece el correo electronico del autor.
    /// </summary>
    [Required]
    [EmailAddress]
    [StringLength(200)]
    public string Email { get; set; } = string.Empty;
}
