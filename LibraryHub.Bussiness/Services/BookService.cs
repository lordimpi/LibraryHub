using LibraryHub.Bussiness.Interfaces;
using LibraryHub.Common.DTOs;
using LibraryHub.Common.Entities;
using LibraryHub.Common.Exceptions;
using LibraryHub.Common.IOptionPattern;
using LibraryHub.Common.Options;
using LibraryHub.Common.Pagination;
using LibraryHub.Common.Responses;
using LibraryHub.DataAccess.UnitOfWork;
using Mapster;

namespace LibraryHub.Bussiness.Services;

/// <summary>
/// Implementa la logica de aplicacion para libros.
/// </summary>
public class BookService : IBookService
{
    private readonly ILibraryHubUnitOfWork _unitOfWork;
    private readonly IGenericOptionsService<BusinessRulesOptions> _businessRulesOptions;

    /// <summary>
    /// Inicializa una nueva instancia del servicio de libros.
    /// </summary>
    /// <param name="unitOfWork">Unidad de trabajo para operaciones de persistencia.</param>
    /// <param name="businessRulesOptions">Servicio de acceso a opciones de reglas de negocio.</param>
    public BookService(
        ILibraryHubUnitOfWork unitOfWork,
        IGenericOptionsService<BusinessRulesOptions> businessRulesOptions)
    {
        _unitOfWork = unitOfWork;
        _businessRulesOptions = businessRulesOptions;
    }

    /// <inheritdoc />
    /// <exception cref="MaxBooksExceededException">Se lanza cuando se supera el maximo permitido de libros.</exception>
    /// <exception cref="AuthorNotFoundException">Se lanza cuando el autor asociado no existe.</exception>
    public async Task<BookDto> CreateAsync(CreateBookDto book, CancellationToken cancellationToken = default)
    {
        var businessRules = _businessRulesOptions.GetMonitorOptions();
        var maxBooksAllowed = businessRules.MaxBooksAllowed;

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var currentBooksCount = await _unitOfWork.Books.CountAsync(cancellationToken);
            if (currentBooksCount >= maxBooksAllowed)
            {
                throw new MaxBooksExceededException("No es posible registrar el libro, se alcanzó el máximo permitido.");
            }

            var author = await _unitOfWork.Authors.GetByIdAsync(book.AuthorId, cancellationToken);
            if (author is null)
            {
                throw new AuthorNotFoundException("El autor no está registrado");
            }

            var bookEntity = book.Adapt<BookEntity>();
            await _unitOfWork.Books.AddAsync(bookEntity, cancellationToken);
            await _unitOfWork.CommitAsync();

            bookEntity.Author = author;
            return bookEntity.Adapt<BookDto>();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<BookDto> GetByIdAsync(int bookId, CancellationToken cancellationToken = default)
    {
        var book = await _unitOfWork.Books.GetByIdAsync(bookId, cancellationToken);
        if (book is null)
        {
            throw new BookNotFoundException("El libro no está registrado");
        }

        return book.Adapt<BookDto>();
    }

    /// <inheritdoc />
    public async Task<BookDto> UpdateAsync(
        int bookId,
        UpdateBookDto book,
        CancellationToken cancellationToken = default)
    {
        var bookEntity = await _unitOfWork.Books.GetByIdAsync(bookId, cancellationToken);
        if (bookEntity is null)
        {
            throw new BookNotFoundException("El libro no está registrado");
        }

        var authorExists = await _unitOfWork.Authors.ExistsByIdAsync(book.AuthorId, cancellationToken);
        if (!authorExists)
        {
            throw new AuthorNotFoundException("El autor no está registrado");
        }

        bookEntity.Title = book.Title;
        bookEntity.Year = book.Year;
        bookEntity.Genre = book.Genre;
        bookEntity.Pages = book.Pages;
        bookEntity.AuthorId = book.AuthorId;
        bookEntity.Author = await _unitOfWork.Authors.GetByIdAsync(book.AuthorId, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return bookEntity.Adapt<BookDto>();
    }

    /// <inheritdoc />
    public async Task SoftDeleteAsync(int bookId, CancellationToken cancellationToken = default)
    {
        var book = await _unitOfWork.Books.GetByIdAsync(bookId, cancellationToken);
        if (book is null)
        {
            throw new BookNotFoundException("El libro no está registrado");
        }

        book.IsDeleted = true;
        book.DeletedAtUtc = DateTime.UtcNow;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<BookDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        _ = _businessRulesOptions.GetMonitorOptions();

        var books = await _unitOfWork.Books.GetAllAsync(cancellationToken);
        return books.Adapt<List<BookDto>>();
    }

    /// <inheritdoc />
    public async Task<PagedResponse<BookDto>> GetPagedAsync(
        PaginationRequest request,
        CancellationToken cancellationToken = default)
    {
        _ = _businessRulesOptions.GetMonitorOptions();

        var searchTerm = NormalizeSearchTerm(request.SearchTerm);

        var pagedBooks = request.Source == PaginationSource.Sp
            ? await _unitOfWork.Books.GetPagedSpAsync(request.PageNumber, request.PageSize, searchTerm, cancellationToken)
            : await _unitOfWork.Books.GetPagedEfAsync(request.PageNumber, request.PageSize, searchTerm, cancellationToken);

        var items = pagedBooks.Items.Adapt<List<BookDto>>();
        return new PagedResponse<BookDto>(items, pagedBooks.TotalCount, request.PageNumber, request.PageSize);
    }

    private static string? NormalizeSearchTerm(string? searchTerm)
    {
        var normalized = searchTerm?.Trim();
        return string.IsNullOrWhiteSpace(normalized) ? null : normalized;
    }
}
