using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace LibraryHub.WebAPI.Tests.Integration;

/// <summary>
/// Fabrica de aplicacion para pruebas de integracion de Web API.
/// </summary>
public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    /// <summary>
    /// Configura el host de pruebas para evitar inicializaciones de desarrollo.
    /// </summary>
    /// <param name="builder">Builder del host web.</param>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
    }
}
