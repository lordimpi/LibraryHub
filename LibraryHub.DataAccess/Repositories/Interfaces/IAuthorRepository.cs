using LibraryHub.Common.Entities;

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
    /// <param name="cancellationToken">Token de cancelación.</param>
    Task AddAsync(AuthorEntity author, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene todos los autores almacenados.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Lista de autores.</returns>
    Task<IReadOnlyCollection<AuthorEntity>> GetAllAsync(CancellationToken cancellationToken = default);
}
