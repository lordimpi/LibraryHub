using LibraryHub.Common.Entities;
using LibraryHub.DataAccess.Context;
using LibraryHub.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryHub.DataAccess.Repositories.Implementations;

/// <summary>
/// Implementa acceso a datos para la entidad de autor.
/// </summary>
public class AuthorRepository : IAuthorRepository
{
    private readonly LibraryHubDbContext _dbContext;

    /// <summary>
    /// Inicializa una nueva instancia del repositorio de autores.
    /// </summary>
    /// <param name="dbContext">Contexto de datos de LibraryHub.</param>
    public AuthorRepository(LibraryHubDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task AddAsync(AuthorEntity author, CancellationToken cancellationToken = default)
    {
        await _dbContext.Authors.AddAsync(author, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<AuthorEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Authors.AsNoTracking().ToListAsync(cancellationToken);
    }
}
