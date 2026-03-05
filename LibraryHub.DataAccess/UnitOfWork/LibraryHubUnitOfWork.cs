using LibraryHub.DataAccess.Context;
using LibraryHub.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace LibraryHub.DataAccess.UnitOfWork;

/// <summary>
/// Implementa la unidad de trabajo para la persistencia de LibraryHub.
/// </summary>
public class LibraryHubUnitOfWork : ILibraryHubUnitOfWork
{
    private readonly LibraryHubDbContext _dbContext;
    private IDbContextTransaction? _currentTransaction;

    /// <summary>
    /// Inicializa una nueva instancia de la unidad de trabajo.
    /// </summary>
    /// <param name="dbContext">Contexto de datos.</param>
    /// <param name="authorRepository">Repositorio de autores.</param>
    /// <param name="bookRepository">Repositorio de libros.</param>
    public LibraryHubUnitOfWork(
        LibraryHubDbContext dbContext,
        IAuthorRepository authorRepository,
        IBookRepository bookRepository)
    {
        _dbContext = dbContext;
        Authors = authorRepository;
        Books = bookRepository;
    }

    /// <inheritdoc />
    public IAuthorRepository Authors { get; }

    /// <inheritdoc />
    public IBookRepository Books { get; }

    /// <inheritdoc />
    public async Task BeginTransactionAsync()
    {
        if (_currentTransaction is not null)
        {
            return;
        }

        _currentTransaction = await _dbContext.Database.BeginTransactionAsync();
    }

    /// <inheritdoc />
    public async Task CommitAsync()
    {
        if (_currentTransaction is null)
        {
            throw new InvalidOperationException("No hay una transacción activa para confirmar.");
        }

        await _dbContext.SaveChangesAsync();
        await _currentTransaction.CommitAsync();
        await _currentTransaction.DisposeAsync();
        _currentTransaction = null;
    }

    /// <inheritdoc />
    public async Task RollbackAsync()
    {
        if (_currentTransaction is null)
        {
            return;
        }

        await _currentTransaction.RollbackAsync();
        await _currentTransaction.DisposeAsync();
        _currentTransaction = null;
    }

    /// <inheritdoc />
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
