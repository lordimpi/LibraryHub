using LibraryHub.Common.Entities;
using LibraryHub.Common.Pagination;

namespace LibraryHub.DataAccess.Repositories.Interfaces;

/// <summary>
/// Define las operaciones de persistencia para autores.
/// </summary>
public interface IAuthorRepository
{
    /// <summary>
    /// Agrega un autor al contexto actual.
    /// </summary>
    /// <param name="author">Entidad de autor a agregar.</param>
    /// <param name="cancellationToken">Token de cancelacion.</param>
    Task AddAsync(AuthorEntity author, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica si existe un autor con el identificador indicado.
    /// </summary>
    /// <param name="authorId">Identificador del autor.</param>
    /// <param name="cancellationToken">Token de cancelacion.</param>
    /// <returns><c>true</c> si el autor existe; en caso contrario, <c>false</c>.</returns>
    Task<bool> ExistsByIdAsync(int authorId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un autor por su identificador.
    /// </summary>
    /// <param name="authorId">Identificador del autor.</param>
    /// <param name="cancellationToken">Token de cancelacion.</param>
    /// <returns>Entidad del autor o <c>null</c> si no existe.</returns>
    Task<AuthorEntity?> GetByIdAsync(int authorId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene todos los autores almacenados.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelacion.</param>
    /// <returns>Lista de autores.</returns>
    Task<IReadOnlyCollection<AuthorEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene autores paginados utilizando EF Core.
    /// </summary>
    /// <param name="pageNumber">Numero de pagina.</param>
    /// <param name="pageSize">Tamano de pagina.</param>
    /// <param name="searchTerm">Termino opcional de busqueda.</param>
    /// <param name="cancellationToken">Token de cancelacion.</param>
    /// <returns>Resultado paginado con total de registros.</returns>
    Task<PagedQueryResult<AuthorEntity>> GetPagedEfAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene autores paginados mediante procedimiento almacenado.
    /// </summary>
    /// <param name="pageNumber">Numero de pagina.</param>
    /// <param name="pageSize">Tamano de pagina.</param>
    /// <param name="searchTerm">Termino opcional de busqueda.</param>
    /// <param name="cancellationToken">Token de cancelacion.</param>
    /// <returns>Resultado paginado con total de registros.</returns>
    Task<PagedQueryResult<AuthorEntity>> GetPagedSpAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm,
        CancellationToken cancellationToken = default);
}
