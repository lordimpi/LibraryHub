using LibraryHub.Common.Entities;
using LibraryHub.Common.Pagination;
using LibraryHub.DataAccess.Context;
using LibraryHub.DataAccess.Repositories.Interfaces;
using LibraryHub.DataAccess.Utilities.ADO;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LibraryHub.DataAccess.Repositories.Implementations;

/// <summary>
/// Implementa acceso a datos para la entidad de libro.
/// </summary>
public class BookRepository : IBookRepository
{
    private readonly LibraryHubDbContext _dbContext;
    private readonly ISqlExecutor _sqlExecutor;

    /// <summary>
    /// Inicializa una nueva instancia del repositorio de libros.
    /// </summary>
    /// <param name="dbContext">Contexto de datos de LibraryHub.</param>
    /// <param name="sqlExecutor">Ejecutor ADO.NET para SQL y SP.</param>
    public BookRepository(LibraryHubDbContext dbContext, ISqlExecutor sqlExecutor)
    {
        _dbContext = dbContext;
        _sqlExecutor = sqlExecutor;
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
            .OrderBy(book => book.Id)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PagedQueryResult<BookEntity>> GetPagedEfAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Books
            .AsNoTracking()
            .Include(book => book.Author)
            .AsQueryable();

        query = ApplySearchFilter(query, searchTerm)
            .OrderBy(book => book.Id);

        var totalCount = await query.CountAsync(cancellationToken);
        var skip = (pageNumber - 1) * pageSize;

        var items = await query
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedQueryResult<BookEntity>(items, totalCount);
    }

    /// <inheritdoc />
    public async Task<PagedQueryResult<BookEntity>> GetPagedSpAsync(
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
            "dbo.sp_Books_GetPaged",
            CommandType.StoredProcedure,
            DataReaderMapper.MapToList<BookPagedSpRow>,
            parameters,
            ct: cancellationToken);

        var totalCount = rows.FirstOrDefault()?.TotalCount ?? 0;
        var items = rows
            .Select(row => new BookEntity
            {
                Id = row.Id,
                Title = row.Title,
                Year = row.Year,
                Genre = row.Genre,
                Pages = row.Pages,
                AuthorId = row.AuthorId,
                Author = new AuthorEntity
                {
                    Id = row.AuthorId,
                    FullName = row.AuthorFullName,
                },
            })
            .ToList();

        return new PagedQueryResult<BookEntity>(items, totalCount);
    }

    private static IQueryable<BookEntity> ApplySearchFilter(IQueryable<BookEntity> query, string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return query;
        }

        var normalizedSearch = searchTerm.Trim().ToLower();
        return query.Where(book =>
            EF.Functions.Like(book.Title.ToLower(), $"%{normalizedSearch}%") ||
            EF.Functions.Like(book.Genre.ToLower(), $"%{normalizedSearch}%") ||
            (book.Author != null && EF.Functions.Like(book.Author.FullName.ToLower(), $"%{normalizedSearch}%")));
    }

    /// <summary>
    /// Representa una fila retornada por el SP de libros paginados.
    /// </summary>
    private sealed class BookPagedSpRow
    {
        /// <summary>
        /// Obtiene o establece el identificador del libro.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece el titulo del libro.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el anio de publicacion.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Obtiene o establece el genero del libro.
        /// </summary>
        public string Genre { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la cantidad de paginas.
        /// </summary>
        public int Pages { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del autor.
        /// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre del autor asociado.
        /// </summary>
        public string AuthorFullName { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el total de registros sin paginar.
        /// </summary>
        public int TotalCount { get; set; }
    }
}
