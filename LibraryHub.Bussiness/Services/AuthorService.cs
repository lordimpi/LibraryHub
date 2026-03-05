using LibraryHub.Bussiness.Interfaces;
using LibraryHub.Common.DTOs;
using LibraryHub.DataAccess.UnitOfWork;
using Mapster;

namespace LibraryHub.Bussiness.Services;

/// <summary>
/// Implementa la lógica de aplicación para autores.
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
    /// <exception cref="NotImplementedException">Se lanza cuando la lógica de creación aún no está implementada.</exception>
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
}
