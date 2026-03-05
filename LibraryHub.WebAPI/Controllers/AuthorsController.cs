using LibraryHub.Bussiness.Interfaces;
using LibraryHub.Common.DTOs;
using LibraryHub.Common.Pagination;
using LibraryHub.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LibraryHub.WebAPI.Controllers;

/// <summary>
/// Expone endpoints para la gestión de autores.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorService _authorService;

    /// <summary>
    /// Inicializa una nueva instancia del controlador de autores.
    /// </summary>
    /// <param name="authorService">Servicio de aplicación para autores.</param>
    public AuthorsController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    /// <summary>
    /// Crea un autor nuevo.
    /// </summary>
    /// <param name="request">Datos de creación del autor.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Resultado de creación del autor.</returns>
    [HttpPost]
    public async Task<ActionResult<AuthorDto>> Create([FromBody] CreateAuthorDto request, CancellationToken cancellationToken)
    {
        var result = await _authorService.CreateAsync(request, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene todos los autores.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Colección de autores.</returns>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<AuthorDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _authorService.GetAllAsync(cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene autores paginados desde EF Core o SP según el origen solicitado.
    /// </summary>
    /// <param name="request">Parámetros de paginación y origen.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Respuesta paginada de autores.</returns>
    [HttpGet("paged")]
    public async Task<ActionResult<PagedResponse<AuthorDto>>> GetPaged(
        [FromQuery] PaginationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _authorService.GetPagedAsync(request, cancellationToken);
        return Ok(result);
    }
}
