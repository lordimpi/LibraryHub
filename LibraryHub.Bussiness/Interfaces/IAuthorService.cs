using LibraryHub.Common.DTOs;
using LibraryHub.Common.Pagination;
using LibraryHub.Common.Responses;

namespace LibraryHub.Bussiness.Interfaces;

/// <summary>
/// Define operaciones de aplicacion para autores.
/// </summary>
public interface IAuthorService
{
    /// <summary>
    /// Crea un autor nuevo en el sistema.
    /// </summary>
    /// <param name="author">Datos de creacion del autor.</param>
    /// <param name="cancellationToken">Token de cancelacion.</param>
    /// <returns>Autor creado.</returns>
    Task<AuthorDto> CreateAsync(CreateAuthorDto author, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un autor por su identificador.
    /// </summary>
    /// <param name="authorId">Identificador del autor.</param>
    /// <param name="cancellationToken">Token de cancelacion.</param>
    /// <returns>Autor encontrado.</returns>
    Task<AuthorDto> GetByIdAsync(int authorId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Actualiza los datos de un autor.
    /// </summary>
    /// <param name="authorId">Identificador del autor.</param>
    /// <param name="author">Datos de actualizacion del autor.</param>
    /// <param name="cancellationToken">Token de cancelacion.</param>
    /// <returns>Autor actualizado.</returns>
    Task<AuthorDto> UpdateAsync(int authorId, UpdateAuthorDto author, CancellationToken cancellationToken = default);

    /// <summary>
    /// Realiza eliminacion logica de un autor y sus libros.
    /// </summary>
    /// <param name="authorId">Identificador del autor.</param>
    /// <param name="cancellationToken">Token de cancelacion.</param>
    Task SoftDeleteAsync(int authorId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene todos los autores disponibles.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelacion.</param>
    /// <returns>Coleccion de autores.</returns>
    Task<IReadOnlyCollection<AuthorDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene autores paginados segun la fuente configurada.
    /// </summary>
    /// <param name="request">Parametros de paginacion solicitados.</param>
    /// <param name="cancellationToken">Token de cancelacion.</param>
    /// <returns>Respuesta paginada de autores.</returns>
    Task<PagedResponse<AuthorDto>> GetPagedAsync(PaginationRequest request, CancellationToken cancellationToken = default);
}
