using LibraryHub.Common.Entities;
using LibraryHub.DataAccess.Context;
using LibraryHub.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryHub.DataAccess.Repositories.Implementations;

/// <summary>
/// Implementa acceso a datos para la entidad de libro.
/// </summary>
public class BookRepository : IBookRepository
{
    private readonly LibraryHubDbContext _dbContext;

    /// <summary>
    /// Inicializa una nueva instancia del repositorio de libros.
    /// </summary>
    /// <param name="dbContext">Contexto de datos de LibraryHub.</param>
    public BookRepository(LibraryHubDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task AddAsync(BookEntity book, CancellationToken cancellationToken = default)
    {
        await _dbContext.Books.AddAsync(book, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<BookEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Books
            .AsNoTracking()
            .Include(book => book.Author)
            .ToListAsync(cancellationToken);
    }
}
