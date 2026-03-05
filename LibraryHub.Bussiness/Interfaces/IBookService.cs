using LibraryHub.Common.DTOs;
using LibraryHub.Common.Pagination;
using LibraryHub.Common.Responses;

namespace LibraryHub.Bussiness.Interfaces;

/// <summary>
/// Define operaciones de aplicación para libros.
/// </summary>
public interface IBookService
{
    /// <summary>
    /// Crea un libro nuevo en el sistema.
    /// </summary>
    /// <param name="book">Datos de creación del libro.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Libro creado.</returns>
    Task<BookDto> CreateAsync(CreateBookDto book, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene todos los libros disponibles.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Colección de libros.</returns>
    Task<IReadOnlyCollection<BookDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene libros paginados según la fuente configurada.
    /// </summary>
    /// <param name="request">Parámetros de paginación solicitados.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Respuesta paginada de libros.</returns>
    Task<PagedResponse<BookDto>> GetPagedAsync(PaginationRequest request, CancellationToken cancellationToken = default);
}
