using LibraryHub.Common.DTOs;
using LibraryHub.Common.Pagination;
using LibraryHub.Common.Responses;

namespace LibraryHub.Bussiness.Interfaces;

/// <summary>
/// Define operaciones de aplicacion para libros.
/// </summary>
public interface IBookService
{
    /// <summary>
    /// Crea un libro nuevo en el sistema.
    /// </summary>
    /// <param name="book">Datos de creacion del libro.</param>
    /// <param name="cancellationToken">Token de cancelacion.</param>
    /// <returns>Libro creado.</returns>
    Task<BookDto> CreateAsync(CreateBookDto book, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un libro por su identificador.
    /// </summary>
    /// <param name="bookId">Identificador del libro.</param>
    /// <param name="cancellationToken">Token de cancelacion.</param>
    /// <returns>Libro encontrado.</returns>
    Task<BookDto> GetByIdAsync(int bookId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Actualiza los datos de un libro.
    /// </summary>
    /// <param name="bookId">Identificador del libro.</param>
    /// <param name="book">Datos de actualizacion del libro.</param>
    /// <param name="cancellationToken">Token de cancelacion.</param>
    /// <returns>Libro actualizado.</returns>
    Task<BookDto> UpdateAsync(int bookId, UpdateBookDto book, CancellationToken cancellationToken = default);

    /// <summary>
    /// Realiza eliminacion logica de un libro.
    /// </summary>
    /// <param name="bookId">Identificador del libro.</param>
    /// <param name="cancellationToken">Token de cancelacion.</param>
    Task SoftDeleteAsync(int bookId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene todos los libros disponibles.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelacion.</param>
    /// <returns>Coleccion de libros.</returns>
    Task<IReadOnlyCollection<BookDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene libros paginados segun la fuente configurada.
    /// </summary>
    /// <param name="request">Parametros de paginacion solicitados.</param>
    /// <param name="cancellationToken">Token de cancelacion.</param>
    /// <returns>Respuesta paginada de libros.</returns>
    Task<PagedResponse<BookDto>> GetPagedAsync(PaginationRequest request, CancellationToken cancellationToken = default);
}
