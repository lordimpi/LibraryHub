using LibraryHub.DataAccess.Repositories.Interfaces;

namespace LibraryHub.DataAccess.UnitOfWork;

/// <summary>
/// Define la unidad de trabajo para coordinar repositorios y persistencia.
/// </summary>
public interface ILibraryHubUnitOfWork
{
    /// <summary>
    /// Obtiene el repositorio de autores.
    /// </summary>
    IAuthorRepository Authors { get; }

    /// <summary>
    /// Obtiene el repositorio de libros.
    /// </summary>
    IBookRepository Books { get; }

    /// <summary>
    /// Inicia una transacción explícita sobre el contexto actual.
    /// </summary>
    Task BeginTransactionAsync();

    /// <summary>
    /// Confirma la transacción actual y persiste cambios pendientes.
    /// </summary>
    Task CommitAsync();

    /// <summary>
    /// Revierte la transacción actual.
    /// </summary>
    Task RollbackAsync();

    /// <summary>
    /// Persiste los cambios pendientes en el contexto.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Cantidad de registros afectados.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
