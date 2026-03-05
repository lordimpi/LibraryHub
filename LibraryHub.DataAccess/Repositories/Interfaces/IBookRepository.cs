using LibraryHub.Common.Entities;
using LibraryHub.Common.Pagination;

namespace LibraryHub.DataAccess.Repositories.Interfaces;

/// <summary>
/// Define las operaciones de persistencia para libros.
/// </summary>
public interface IBookRepository
{
    /// <summary>
    /// Agrega un libro al contexto actual.
    /// </summary>
    /// <param name="book">Entidad de libro a agregar.</param>
    /// <param name="cancellationToken">Token de cancelacion.</param>
    Task AddAsync(BookEntity book, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene la cantidad total de libros activos (no eliminados logicamente).
    /// </summary>
    /// <param name="cancellationToken">Token de cancelacion.</param>
    /// <returns>Total de libros activos.</returns>
    Task<int> CountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un libro por su identificador.
    /// </summary>
    /// <param name="bookId">Identificador del libro.</param>
    /// <param name="cancellationToken">Token de cancelacion.</param>
    /// <returns>Entidad del libro o <c>null</c> si no existe.</returns>
    Task<BookEntity?> GetByIdAsync(int bookId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene los libros activos de un autor.
    /// </summary>
    /// <param name="authorId">Identificador del autor.</param>
    /// <param name="cancellationToken">Token de cancelacion.</param>
    /// <returns>Coleccion de libros del autor.</returns>
    Task<IReadOnlyCollection<BookEntity>> GetByAuthorIdAsync(int authorId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene todos los libros almacenados.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelacion.</param>
    /// <returns>Lista de libros.</returns>
    Task<IReadOnlyCollection<BookEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene libros paginados utilizando EF Core.
    /// </summary>
    /// <param name="pageNumber">Numero de pagina.</param>
    /// <param name="pageSize">Tamano de pagina.</param>
    /// <param name="searchTerm">Termino opcional de busqueda.</param>
    /// <param name="cancellationToken">Token de cancelacion.</param>
    /// <returns>Resultado paginado con total de registros.</returns>
    Task<PagedQueryResult<BookEntity>> GetPagedEfAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene libros paginados mediante procedimiento almacenado.
    /// </summary>
    /// <param name="pageNumber">Numero de pagina.</param>
    /// <param name="pageSize">Tamano de pagina.</param>
    /// <param name="searchTerm">Termino opcional de busqueda.</param>
    /// <param name="cancellationToken">Token de cancelacion.</param>
    /// <returns>Resultado paginado con total de registros.</returns>
    Task<PagedQueryResult<BookEntity>> GetPagedSpAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm,
        CancellationToken cancellationToken = default);
}
