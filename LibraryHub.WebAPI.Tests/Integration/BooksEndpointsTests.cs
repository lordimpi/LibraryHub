using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace LibraryHub.WebAPI.Tests.Integration;

/// <summary>
/// Contiene pruebas de integracion minimas para endpoints de libros.
/// </summary>
public class BooksEndpointsTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Inicializa una nueva instancia de la clase de pruebas.
    /// </summary>
    /// <param name="factory">Fabrica del host de pruebas.</param>
    public BooksEndpointsTests(TestWebApplicationFactory factory)
    {
        _httpClient = factory.CreateClient();
    }

    /// <summary>
    /// Verifica que un payload invalido en creacion de libro responda 400.
    /// </summary>
    [Fact]
    public async Task PostBooks_ShouldReturnBadRequest_WhenPayloadIsInvalid()
    {
        var payload = new
        {
            Title = string.Empty,
            Year = 0,
            Genre = string.Empty,
            Pages = 0,
            AuthorId = 0,
        };

        var response = await _httpClient.PostAsJsonAsync("/api/books", payload);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
