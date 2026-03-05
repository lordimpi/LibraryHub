using FluentAssertions;
using LibraryHub.Bussiness.Interfaces;
using LibraryHub.Common.DTOs;
using LibraryHub.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibraryHub.WebAPI.Tests.Controllers;

/// <summary>
/// Contiene pruebas unitarias para <see cref="AuthorsController"/>.
/// </summary>
public class AuthorsControllerTests
{
    /// <summary>
    /// Verifica que la accion Create retorne 200 con el autor creado.
    /// </summary>
    [Fact]
    public async Task Create_ShouldReturnOk_WhenAuthorIsCreated()
    {
        var request = new CreateAuthorDto
        {
            FullName = "Autor Test",
            BirthDate = new DateTime(1990, 1, 1),
            City = "Bogota",
            Email = "autor@test.com",
        };

        var expected = new AuthorDto
        {
            Id = 1,
            FullName = request.FullName,
            BirthDate = request.BirthDate,
            City = request.City,
            Email = request.Email,
        };

        var serviceMock = new Mock<IAuthorService>();
        serviceMock
            .Setup(service => service.CreateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var controller = new AuthorsController(serviceMock.Object);

        var result = await controller.Create(request, CancellationToken.None);

        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result.Result!;
        okResult.Value.Should().BeEquivalentTo(expected);
    }

    /// <summary>
    /// Verifica que la accion GetById retorne 200 cuando el autor existe.
    /// </summary>
    [Fact]
    public async Task GetById_ShouldReturnOk_WhenAuthorExists()
    {
        const int authorId = 7;
        var expected = new AuthorDto
        {
            Id = authorId,
            FullName = "Autor",
            BirthDate = new DateTime(1985, 5, 5),
            City = "Medellin",
            Email = "autor@demo.com",
        };

        var serviceMock = new Mock<IAuthorService>();
        serviceMock
            .Setup(service => service.GetByIdAsync(authorId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var controller = new AuthorsController(serviceMock.Object);

        var result = await controller.GetById(authorId, CancellationToken.None);

        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result.Result!;
        okResult.Value.Should().BeEquivalentTo(expected);
    }
}
