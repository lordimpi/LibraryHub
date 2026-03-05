using LibraryHub.Common.DTOs;
using LibraryHub.Common.Pagination;
using LibraryHub.Common.Responses;

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

    /// <summary>
    /// Obtiene autores paginados según la fuente configurada.
    /// </summary>
    /// <param name="request">Parámetros de paginación solicitados.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Respuesta paginada de autores.</returns>
    Task<PagedResponse<AuthorDto>> GetPagedAsync(PaginationRequest request, CancellationToken cancellationToken = default);
}
