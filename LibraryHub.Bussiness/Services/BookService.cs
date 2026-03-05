using LibraryHub.Bussiness.Interfaces;
using LibraryHub.Common.DTOs;
using LibraryHub.Common.IOptionPattern;
using LibraryHub.Common.Options;
using LibraryHub.DataAccess.UnitOfWork;
using Mapster;

namespace LibraryHub.Bussiness.Services;

/// <summary>
/// Implementa la lógica de aplicación para libros.
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
    /// <exception cref="NotImplementedException">Se lanza cuando la lógica de creación aún no está implementada.</exception>
    public Task<BookDto> CreateAsync(CreateBookDto book, CancellationToken cancellationToken = default)
    {
        _ = _businessRulesOptions.GetMonitorOptions();
        throw new NotImplementedException("Book creation business logic is not implemented yet.");
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<BookDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        _ = _businessRulesOptions.GetMonitorOptions();

        var books = await _unitOfWork.Books.GetAllAsync(cancellationToken);
        return books.Adapt<List<BookDto>>();
    }
}
