using LibraryHub.Common.DTOs;

namespace LibraryHub.Bussiness.Interfaces;

/// <summary>
/// Define operaciones de aplicación para autores.
/// </summary>
public interface IAuthorService
{
    /// <summary>
    /// Crea un autor nuevo en el sistema.
    /// </summary>
    /// <param name="author">Datos de creación del autor.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Autor creado.</returns>
    Task<AuthorDto> CreateAsync(CreateAuthorDto author, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene todos los autores disponibles.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Colección de autores.</returns>
    Task<IReadOnlyCollection<AuthorDto>> GetAllAsync(CancellationToken cancellationToken = default);
}
