using FluentAssertions;
using LibraryHub.Bussiness.Interfaces;
using LibraryHub.Common.DTOs;
using LibraryHub.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibraryHub.WebAPI.Tests.Controllers;

/// <summary>
/// Contiene pruebas unitarias para <see cref="BooksController"/>.
/// </summary>
public class BooksControllerTests
{
    /// <summary>
    /// Verifica que la accion Create retorne 200 con el libro creado.
    /// </summary>
    [Fact]
    public async Task Create_ShouldReturnOk_WhenBookIsCreated()
    {
        var request = new CreateBookDto
        {
            Title = "Libro Test",
            Year = 2024,
            Genre = "Tecnico",
            Pages = 120,
            AuthorId = 1,
        };

        var expected = new BookDto
        {
            Id = 20,
            Title = request.Title,
            Year = request.Year,
            Genre = request.Genre,
            Pages = request.Pages,
            AuthorId = request.AuthorId,
            AuthorFullName = "Autor Test",
        };

        var serviceMock = new Mock<IBookService>();
        serviceMock
            .Setup(service => service.CreateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var controller = new BooksController(serviceMock.Object);

        var result = await controller.Create(request, CancellationToken.None);

        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result.Result!;
        okResult.Value.Should().BeEquivalentTo(expected);
    }

    /// <summary>
    /// Verifica que la accion SoftDelete retorne 200 y ejecute el servicio.
    /// </summary>
    [Fact]
    public async Task SoftDelete_ShouldReturnOk_WhenServiceCompletes()
    {
        const int bookId = 99;
        var serviceMock = new Mock<IBookService>();
        serviceMock
            .Setup(service => service.SoftDeleteAsync(bookId, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var controller = new BooksController(serviceMock.Object);

        var result = await controller.SoftDelete(bookId, CancellationToken.None);

        result.Should().BeOfType<OkResult>();
        serviceMock.Verify(
            service => service.SoftDeleteAsync(bookId, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
