using LibraryHub.Bussiness.Interfaces;
using LibraryHub.Common.DTOs;
using LibraryHub.Common.Pagination;
using LibraryHub.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LibraryHub.WebAPI.Controllers;

/// <summary>
/// Expone endpoints para la gestion de autores.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorService _authorService;

    /// <summary>
    /// Inicializa una nueva instancia del controlador de autores.
    /// </summary>
    /// <param name="authorService">Servicio de aplicacion para autores.</param>
    public AuthorsController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    /// <summary>
    /// Crea un autor nuevo.
    /// </summary>
    /// <param name="request">Datos de creacion del autor.</param>
    /// <param name="cancellationToken">Token de cancelacion.</param>
    /// <returns>Resultado de creacion del autor.</returns>
    [HttpPost]
    public async Task<ActionResult<AuthorDto>> Create([FromBody] CreateAuthorDto request, CancellationToken cancellationToken)
    {
        var result = await _authorService.CreateAsync(request, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene un autor por su identificador.
    /// </summary>
    /// <param name="id">Identificador del autor.</param>
    /// <param name="cancellationToken">Token de cancelacion.</param>
    /// <returns>Autor encontrado.</returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AuthorDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _authorService.GetByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Actualiza un autor existente.
    /// </summary>
    /// <param name="id">Identificador del autor.</param>
    /// <param name="request">Datos de actualizacion del autor.</param>
    /// <param name="cancellationToken">Token de cancelacion.</param>
    /// <returns>Autor actualizado.</returns>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<AuthorDto>> Update(
        int id,
        [FromBody] UpdateAuthorDto request,
        CancellationToken cancellationToken)
    {
        var result = await _authorService.UpdateAsync(id, request, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Realiza eliminacion logica de un autor y sus libros.
    /// </summary>
    /// <param name="id">Identificador del autor.</param>
    /// <param name="cancellationToken">Token de cancelacion.</param>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> SoftDelete(int id, CancellationToken cancellationToken)
    {
        await _authorService.SoftDeleteAsync(id, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Obtiene todos los autores.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelacion.</param>
    /// <returns>Coleccion de autores.</returns>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<AuthorDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _authorService.GetAllAsync(cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene autores paginados desde EF Core o SP segun el origen solicitado.
    /// </summary>
    /// <param name="request">Parametros de paginacion y origen.</param>
    /// <param name="cancellationToken">Token de cancelacion.</param>
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
