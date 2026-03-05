using FluentAssertions;
using LibraryHub.Common.Exceptions;
using LibraryHub.WebAPI.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text;

namespace LibraryHub.WebAPI.Tests.Middleware;

/// <summary>
/// Contiene pruebas unitarias para <see cref="GlobalExceptionMiddleware"/>.
/// </summary>
public class GlobalExceptionMiddlewareTests
{
    /// <summary>
    /// Verifica que una excepcion de autor no encontrado responda 400.
    /// </summary>
    [Fact]
    public async Task InvokeAsync_ShouldReturnBadRequest_WhenAuthorNotFoundExceptionIsThrown()
    {
        var (statusCode, body) = await ExecuteMiddlewareAsync(new AuthorNotFoundException("El autor no está registrado"));

        statusCode.Should().Be(StatusCodes.Status400BadRequest);
        body.Should().Contain("El autor no está registrado");
    }

    /// <summary>
    /// Verifica que una excepcion de maximo de libros responda 400.
    /// </summary>
    [Fact]
    public async Task InvokeAsync_ShouldReturnBadRequest_WhenMaxBooksExceededExceptionIsThrown()
    {
        var (statusCode, body) = await ExecuteMiddlewareAsync(
            new MaxBooksExceededException("No es posible registrar el libro, se alcanzó el máximo permitido."));

        statusCode.Should().Be(StatusCodes.Status400BadRequest);
        body.Should().Contain("No es posible registrar el libro, se alcanzó el máximo permitido.");
    }

    /// <summary>
    /// Verifica que una excepcion de libro no encontrado responda 400.
    /// </summary>
    [Fact]
    public async Task InvokeAsync_ShouldReturnBadRequest_WhenBookNotFoundExceptionIsThrown()
    {
        var (statusCode, body) = await ExecuteMiddlewareAsync(new BookNotFoundException("El libro no está registrado"));

        statusCode.Should().Be(StatusCodes.Status400BadRequest);
        body.Should().Contain("El libro no está registrado");
    }

    /// <summary>
    /// Verifica que una excepcion no controlada responda 500.
    /// </summary>
    [Fact]
    public async Task InvokeAsync_ShouldReturnInternalServerError_WhenUnhandledExceptionIsThrown()
    {
        var (statusCode, body) = await ExecuteMiddlewareAsync(new Exception("Error no controlado"));

        statusCode.Should().Be(StatusCodes.Status500InternalServerError);
        body.Should().Contain("Internal Server Error");
    }

    private static async Task<(int StatusCode, string Body)> ExecuteMiddlewareAsync(Exception exceptionToThrow)
    {
        var loggerMock = new Mock<ILogger<GlobalExceptionMiddleware>>();
        RequestDelegate next = _ => throw exceptionToThrow;

        var middleware = new GlobalExceptionMiddleware(next, loggerMock.Object);
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        await middleware.InvokeAsync(context);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(context.Response.Body, Encoding.UTF8, leaveOpen: true);
        var body = await reader.ReadToEndAsync();

        return (context.Response.StatusCode, body);
    }
}
