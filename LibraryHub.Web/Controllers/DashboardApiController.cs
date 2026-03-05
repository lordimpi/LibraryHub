using System.Globalization;
using System.Text.Json;
using LibraryHub.Bussiness.Interfaces;
using LibraryHub.Common.DTOs;
using LibraryHub.Common.Pagination;
using LibraryHub.Common.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace LibraryHub.Web.Controllers;

/// <summary>
/// Expone endpoints BFF para el dashboard SPA de LibraryHub.
/// </summary>
[ApiController]
[Route("dashboard-api")]
public class DashboardApiController : ControllerBase
{
    private readonly IHttpApiService _httpApiService;

    /// <summary>
    /// Inicializa una nueva instancia del controlador BFF del dashboard.
    /// </summary>
    /// <param name="httpApiService">Servicio HTTP para consumir el backend Web API.</param>
    public DashboardApiController(IHttpApiService httpApiService)
    {
        _httpApiService = httpApiService;
    }

    /// <summary>
    /// Obtiene autores paginados desde el backend.
    /// </summary>
    /// <param name="request">Parámetros de paginación y filtro.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Respuesta paginada de autores.</returns>
    [HttpGet("authors/paged")]
    public async Task<ActionResult<PagedResponse<AuthorDto>>> GetAuthorsPaged(
        [FromQuery] PaginationRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var url = BuildPagedEndpoint("authors", request);
            var response = await _httpApiService.GetAsync<PagedResponse<AuthorDto>>(url, cancellationToken);
            return Ok(response);
        }
        catch (HttpRequestException exception)
        {
            return BuildErrorResponse(exception);
        }
    }

    /// <summary>
    /// Obtiene un autor por identificador.
    /// </summary>
    /// <param name="id">Identificador del autor.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Autor encontrado.</returns>
    [HttpGet("authors/{id:int}")]
    public async Task<ActionResult<AuthorDto>> GetAuthorById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpApiService.GetAsync<AuthorDto>($"api/authors/{id}", cancellationToken);
            return Ok(response);
        }
        catch (HttpRequestException exception)
        {
            return BuildErrorResponse(exception);
        }
    }

    /// <summary>
    /// Crea un autor nuevo.
    /// </summary>
    /// <param name="request">Datos del autor a crear.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Autor creado.</returns>
    [HttpPost("authors")]
    public async Task<ActionResult<AuthorDto>> CreateAuthor(
        [FromBody] CreateAuthorDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpApiService.PostAsync<CreateAuthorDto, AuthorDto>(
                "api/authors",
                request,
                cancellationToken);

            return Ok(response);
        }
        catch (HttpRequestException exception)
        {
            return BuildErrorResponse(exception);
        }
    }

    /// <summary>
    /// Actualiza un autor existente.
    /// </summary>
    /// <param name="id">Identificador del autor.</param>
    /// <param name="request">Datos actualizados del autor.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Autor actualizado.</returns>
    [HttpPut("authors/{id:int}")]
    public async Task<ActionResult<AuthorDto>> UpdateAuthor(
        int id,
        [FromBody] UpdateAuthorDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpApiService.PutAsync<UpdateAuthorDto, AuthorDto>(
                $"api/authors/{id}",
                request,
                cancellationToken);

            return Ok(response);
        }
        catch (HttpRequestException exception)
        {
            return BuildErrorResponse(exception);
        }
    }

    /// <summary>
    /// Elimina lógicamente un autor.
    /// </summary>
    /// <param name="id">Identificador del autor.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Resultado de la operación.</returns>
    [HttpDelete("authors/{id:int}")]
    public async Task<IActionResult> SoftDeleteAuthor(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _httpApiService.DeleteAsync<object>($"api/authors/{id}", cancellationToken);
            return Ok();
        }
        catch (HttpRequestException exception)
        {
            return BuildErrorResponse(exception);
        }
    }

    /// <summary>
    /// Obtiene libros paginados desde el backend.
    /// </summary>
    /// <param name="request">Parámetros de paginación y filtro.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Respuesta paginada de libros.</returns>
    [HttpGet("books/paged")]
    public async Task<ActionResult<PagedResponse<BookDto>>> GetBooksPaged(
        [FromQuery] PaginationRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var url = BuildPagedEndpoint("books", request);
            var response = await _httpApiService.GetAsync<PagedResponse<BookDto>>(url, cancellationToken);
            return Ok(response);
        }
        catch (HttpRequestException exception)
        {
            return BuildErrorResponse(exception);
        }
    }

    /// <summary>
    /// Obtiene un libro por identificador.
    /// </summary>
    /// <param name="id">Identificador del libro.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Libro encontrado.</returns>
    [HttpGet("books/{id:int}")]
    public async Task<ActionResult<BookDto>> GetBookById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpApiService.GetAsync<BookDto>($"api/books/{id}", cancellationToken);
            return Ok(response);
        }
        catch (HttpRequestException exception)
        {
            return BuildErrorResponse(exception);
        }
    }

    /// <summary>
    /// Crea un libro nuevo.
    /// </summary>
    /// <param name="request">Datos del libro a crear.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Libro creado.</returns>
    [HttpPost("books")]
    public async Task<ActionResult<BookDto>> CreateBook(
        [FromBody] CreateBookDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpApiService.PostAsync<CreateBookDto, BookDto>(
                "api/books",
                request,
                cancellationToken);

            return Ok(response);
        }
        catch (HttpRequestException exception)
        {
            return BuildErrorResponse(exception);
        }
    }

    /// <summary>
    /// Actualiza un libro existente.
    /// </summary>
    /// <param name="id">Identificador del libro.</param>
    /// <param name="request">Datos actualizados del libro.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Libro actualizado.</returns>
    [HttpPut("books/{id:int}")]
    public async Task<ActionResult<BookDto>> UpdateBook(
        int id,
        [FromBody] UpdateBookDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpApiService.PutAsync<UpdateBookDto, BookDto>(
                $"api/books/{id}",
                request,
                cancellationToken);

            return Ok(response);
        }
        catch (HttpRequestException exception)
        {
            return BuildErrorResponse(exception);
        }
    }

    /// <summary>
    /// Elimina lógicamente un libro.
    /// </summary>
    /// <param name="id">Identificador del libro.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Resultado de la operación.</returns>
    [HttpDelete("books/{id:int}")]
    public async Task<IActionResult> SoftDeleteBook(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _httpApiService.DeleteAsync<object>($"api/books/{id}", cancellationToken);
            return Ok();
        }
        catch (HttpRequestException exception)
        {
            return BuildErrorResponse(exception);
        }
    }

    /// <summary>
    /// Construye la ruta paginada para autores o libros.
    /// </summary>
    /// <param name="resourceName">Nombre del recurso.</param>
    /// <param name="request">Parámetros de paginación.</param>
    /// <returns>Ruta relativa hacia el endpoint paginado.</returns>
    private static string BuildPagedEndpoint(string resourceName, PaginationRequest request)
    {
        var query = new Dictionary<string, string?>
        {
            ["pageNumber"] = request.PageNumber.ToString(CultureInfo.InvariantCulture),
            ["pageSize"] = request.PageSize.ToString(CultureInfo.InvariantCulture),
            ["source"] = request.Source.ToString(),
        };

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query["searchTerm"] = request.SearchTerm;
        }

        return QueryHelpers.AddQueryString($"api/{resourceName}/paged", query);
    }

    /// <summary>
    /// Construye la respuesta HTTP a partir de una excepción de llamada remota.
    /// </summary>
    /// <param name="exception">Excepción HTTP capturada.</param>
    /// <returns>Resultado con status code y payload de error.</returns>
    private ActionResult BuildErrorResponse(HttpRequestException exception)
    {
        var statusCode = exception.StatusCode.HasValue
            ? (int)exception.StatusCode.Value
            : StatusCodes.Status502BadGateway;

        if (TryGetJsonPayload(exception.Message, out var payload))
        {
            return new ContentResult
            {
                StatusCode = statusCode,
                ContentType = "application/json",
                Content = payload,
            };
        }

        return StatusCode(
            statusCode,
            new
            {
                message = string.IsNullOrWhiteSpace(exception.Message)
                    ? "Error al comunicarse con el backend."
                    : exception.Message,
            });
    }

    /// <summary>
    /// Determina si el texto contiene un JSON válido.
    /// </summary>
    /// <param name="text">Texto candidato.</param>
    /// <param name="jsonPayload">Texto JSON normalizado cuando aplica.</param>
    /// <returns><c>true</c> cuando el texto representa JSON válido.</returns>
    private static bool TryGetJsonPayload(string text, out string jsonPayload)
    {
        jsonPayload = string.Empty;

        if (string.IsNullOrWhiteSpace(text))
        {
            return false;
        }

        try
        {
            using var document = JsonDocument.Parse(text);
            jsonPayload = document.RootElement.GetRawText();
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
    }
}
