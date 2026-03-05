using LibraryHub.Common.Entities;

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
    /// <param name="cancellationToken">Token de cancelación.</param>
    Task AddAsync(BookEntity book, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene todos los libros almacenados.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Lista de libros.</returns>
    Task<IReadOnlyCollection<BookEntity>> GetAllAsync(CancellationToken cancellationToken = default);
}
