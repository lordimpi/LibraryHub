using LibraryHub.Bussiness.Interfaces;
using LibraryHub.Common.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace LibraryHub.WebAPI.Controllers;

/// <summary>
/// Expone endpoints para la gestión de libros.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    /// <summary>
    /// Inicializa una nueva instancia del controlador de libros.
    /// </summary>
    /// <param name="bookService">Servicio de aplicación para libros.</param>
    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    /// <summary>
    /// Crea un libro nuevo.
    /// </summary>
    /// <param name="request">Datos de creación del libro.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Resultado de creación del libro.</returns>
    [HttpPost]
    public async Task<ActionResult<BookDto>> Create([FromBody] CreateBookDto request, CancellationToken cancellationToken)
    {
        var result = await _bookService.CreateAsync(request, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene todos los libros.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Colección de libros.</returns>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<BookDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _bookService.GetAllAsync(cancellationToken);
        return Ok(result);
    }
}
