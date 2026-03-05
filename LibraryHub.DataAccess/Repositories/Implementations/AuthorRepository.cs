using LibraryHub.Common.Entities;
using LibraryHub.Common.Pagination;
using LibraryHub.DataAccess.Context;
using LibraryHub.DataAccess.Repositories.Interfaces;
using LibraryHub.DataAccess.Utilities.ADO;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LibraryHub.DataAccess.Repositories.Implementations;

/// <summary>
/// Implementa acceso a datos para la entidad de autor.
/// </summary>
public class AuthorRepository : IAuthorRepository
{
    private readonly LibraryHubDbContext _dbContext;
    private readonly ISqlExecutor _sqlExecutor;

    /// <summary>
    /// Inicializa una nueva instancia del repositorio de autores.
    /// </summary>
    /// <param name="dbContext">Contexto de datos de LibraryHub.</param>
    /// <param name="sqlExecutor">Ejecutor ADO.NET para SQL y SP.</param>
    public AuthorRepository(LibraryHubDbContext dbContext, ISqlExecutor sqlExecutor)
    {
        _dbContext = dbContext;
        _sqlExecutor = sqlExecutor;
    }

    /// <inheritdoc />
    public async Task AddAsync(AuthorEntity author, CancellationToken cancellationToken = default)
    {
        await _dbContext.Authors.AddAsync(author, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> ExistsByIdAsync(int authorId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Authors
            .AsNoTracking()
            .AnyAsync(author => author.Id == authorId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<AuthorEntity?> GetByIdAsync(int authorId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Authors
            .FirstOrDefaultAsync(author => author.Id == authorId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<AuthorEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Authors
            .AsNoTracking()
            .OrderBy(author => author.Id)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PagedQueryResult<AuthorEntity>> GetPagedEfAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Authors
            .AsNoTracking()
            .AsQueryable();

        query = ApplySearchFilter(query, searchTerm)
            .OrderBy(author => author.Id);

        var totalCount = await query.CountAsync(cancellationToken);
        var skip = (pageNumber - 1) * pageSize;

        var items = await query
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedQueryResult<AuthorEntity>(items, totalCount);
    }

    /// <inheritdoc />
    public async Task<PagedQueryResult<AuthorEntity>> GetPagedSpAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm,
        CancellationToken cancellationToken = default)
    {
        var parameters = new[]
        {
            SqlParam.In("PageNumber", SqlDbType.Int, pageNumber),
            SqlParam.In("PageSize", SqlDbType.Int, pageSize),
            SqlParam.In("SearchTerm", SqlDbType.NVarChar, DbValue.ToDb(searchTerm)),
        };

        var rows = await _sqlExecutor.QueryAsync(
            "dbo.sp_Authors_GetPaged",
            CommandType.StoredProcedure,
            DataReaderMapper.MapToList<AuthorPagedSpRow>,
            parameters,
            ct: cancellationToken);

        var totalCount = rows.FirstOrDefault()?.TotalCount ?? 0;
        var items = rows
            .Select(row => new AuthorEntity
            {
                Id = row.Id,
                FullName = row.FullName,
                BirthDate = row.BirthDate,
                City = row.City,
                Email = row.Email,
            })
            .ToList();

        return new PagedQueryResult<AuthorEntity>(items, totalCount);
    }

    private static IQueryable<AuthorEntity> ApplySearchFilter(IQueryable<AuthorEntity> query, string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return query;
        }

        var normalizedSearch = searchTerm.Trim().ToLower();
        return query.Where(author =>
            EF.Functions.Like(author.FullName.ToLower(), $"%{normalizedSearch}%") ||
            EF.Functions.Like(author.City.ToLower(), $"%{normalizedSearch}%") ||
            EF.Functions.Like(author.Email.ToLower(), $"%{normalizedSearch}%"));
    }

    /// <summary>
    /// Representa una fila retornada por el SP de autores paginados.
    /// </summary>
    private sealed class AuthorPagedSpRow
    {
        /// <summary>
        /// Obtiene o establece el identificador del autor.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre completo del autor.
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la fecha de nacimiento del autor.
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Obtiene o establece la ciudad del autor.
        /// </summary>
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el correo del autor.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el total de registros sin paginar.
        /// </summary>
        public int TotalCount { get; set; }
    }
}
