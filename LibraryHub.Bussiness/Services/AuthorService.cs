using LibraryHub.Bussiness.Interfaces;
using LibraryHub.Common.DTOs;
using LibraryHub.Common.Entities;
using LibraryHub.Common.Exceptions;
using LibraryHub.Common.Pagination;
using LibraryHub.Common.Responses;
using LibraryHub.DataAccess.UnitOfWork;
using Mapster;

namespace LibraryHub.Bussiness.Services;

/// <summary>
/// Implementa la logica de aplicacion para autores.
/// </summary>
public class AuthorService : IAuthorService
{
    private readonly ILibraryHubUnitOfWork _unitOfWork;

    /// <summary>
    /// Inicializa una nueva instancia del servicio de autores.
    /// </summary>
    /// <param name="unitOfWork">Unidad de trabajo para operaciones de persistencia.</param>
    public AuthorService(ILibraryHubUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task<AuthorDto> CreateAsync(CreateAuthorDto author, CancellationToken cancellationToken = default)
    {
        var authorEntity = author.Adapt<AuthorEntity>();
        await _unitOfWork.Authors.AddAsync(authorEntity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return authorEntity.Adapt<AuthorDto>();
    }

    /// <inheritdoc />
    public async Task<AuthorDto> GetByIdAsync(int authorId, CancellationToken cancellationToken = default)
    {
        var author = await _unitOfWork.Authors.GetByIdAsync(authorId, cancellationToken);
        if (author is null)
        {
            throw new AuthorNotFoundException("El autor no está registrado");
        }

        return author.Adapt<AuthorDto>();
    }

    /// <inheritdoc />
    public async Task<AuthorDto> UpdateAsync(
        int authorId,
        UpdateAuthorDto author,
        CancellationToken cancellationToken = default)
    {
        var authorEntity = await _unitOfWork.Authors.GetByIdAsync(authorId, cancellationToken);
        if (authorEntity is null)
        {
            throw new AuthorNotFoundException("El autor no está registrado");
        }

        authorEntity.FullName = author.FullName;
        authorEntity.BirthDate = author.BirthDate;
        authorEntity.City = author.City;
        authorEntity.Email = author.Email;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return authorEntity.Adapt<AuthorDto>();
    }

    /// <inheritdoc />
    public async Task SoftDeleteAsync(int authorId, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(authorId, cancellationToken);
            if (author is null)
            {
                throw new AuthorNotFoundException("El autor no está registrado");
            }

            var deletedAtUtc = DateTime.UtcNow;
            author.IsDeleted = true;
            author.DeletedAtUtc = deletedAtUtc;

            var authorBooks = await _unitOfWork.Books.GetByAuthorIdAsync(authorId, cancellationToken);
            foreach (var book in authorBooks)
            {
                book.IsDeleted = true;
                book.DeletedAtUtc = deletedAtUtc;
            }

            await _unitOfWork.CommitAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<AuthorDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var authors = await _unitOfWork.Authors.GetAllAsync(cancellationToken);
        return authors.Adapt<List<AuthorDto>>();
    }

    /// <inheritdoc />
    public async Task<PagedResponse<AuthorDto>> GetPagedAsync(
        PaginationRequest request,
        CancellationToken cancellationToken = default)
    {
        var searchTerm = NormalizeSearchTerm(request.SearchTerm);

        var pagedAuthors = request.Source == PaginationSource.Sp
            ? await _unitOfWork.Authors.GetPagedSpAsync(request.PageNumber, request.PageSize, searchTerm, cancellationToken)
            : await _unitOfWork.Authors.GetPagedEfAsync(request.PageNumber, request.PageSize, searchTerm, cancellationToken);

        var items = pagedAuthors.Items.Adapt<List<AuthorDto>>();
        return new PagedResponse<AuthorDto>(items, pagedAuthors.TotalCount, request.PageNumber, request.PageSize);
    }

    private static string? NormalizeSearchTerm(string? searchTerm)
    {
        var normalized = searchTerm?.Trim();
        return string.IsNullOrWhiteSpace(normalized) ? null : normalized;
    }
}
