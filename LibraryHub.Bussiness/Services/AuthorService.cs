using LibraryHub.Bussiness.Interfaces;
using LibraryHub.Common.DTOs;
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
    /// <exception cref="NotImplementedException">Se lanza cuando la logica de creacion aun no esta implementada.</exception>
    public Task<AuthorDto> CreateAsync(CreateAuthorDto author, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("Author creation business logic is not implemented yet.");
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
