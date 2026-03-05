using LibraryHub.Bussiness.Interfaces;
using LibraryHub.Common.DTOs;
using LibraryHub.Common.Pagination;
using LibraryHub.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LibraryHub.WebAPI.Controllers;

/// <summary>
/// Expone endpoints para la gestion de libros.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    /// <summary>
    /// Inicializa una nueva instancia del controlador de libros.
    /// </summary>
    /// <param name="bookService">Servicio de aplicacion para libros.</param>
    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    /// <summary>
    /// Crea un libro nuevo.
    /// </summary>
    /// <param name="request">Datos de creacion del libro.</param>
    /// <param name="cancellationToken">Token de cancelacion.</param>
    /// <returns>Resultado de creacion del libro.</returns>
    [HttpPost]
    public async Task<ActionResult<BookDto>> Create([FromBody] CreateBookDto request, CancellationToken cancellationToken)
    {
        var result = await _bookService.CreateAsync(request, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene un libro por su identificador.
    /// </summary>
    /// <param name="id">Identificador del libro.</param>
    /// <param name="cancellationToken">Token de cancelacion.</param>
    /// <returns>Libro encontrado.</returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _bookService.GetByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Actualiza un libro existente.
    /// </summary>
    /// <param name="id">Identificador del libro.</param>
    /// <param name="request">Datos de actualizacion del libro.</param>
    /// <param name="cancellationToken">Token de cancelacion.</param>
    /// <returns>Libro actualizado.</returns>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<BookDto>> Update(
        int id,
        [FromBody] UpdateBookDto request,
        CancellationToken cancellationToken)
    {
        var result = await _bookService.UpdateAsync(id, request, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Realiza eliminacion logica de un libro.
    /// </summary>
    /// <param name="id">Identificador del libro.</param>
    /// <param name="cancellationToken">Token de cancelacion.</param>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> SoftDelete(int id, CancellationToken cancellationToken)
    {
        await _bookService.SoftDeleteAsync(id, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Obtiene todos los libros.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelacion.</param>
    /// <returns>Coleccion de libros.</returns>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<BookDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _bookService.GetAllAsync(cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene libros paginados desde EF Core o SP segun el origen solicitado.
    /// </summary>
    /// <param name="request">Parametros de paginacion y origen.</param>
    /// <param name="cancellationToken">Token de cancelacion.</param>
    /// <returns>Respuesta paginada de libros.</returns>
    [HttpGet("paged")]
    public async Task<ActionResult<PagedResponse<BookDto>>> GetPaged(
        [FromQuery] PaginationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _bookService.GetPagedAsync(request, cancellationToken);
        return Ok(result);
    }
}
