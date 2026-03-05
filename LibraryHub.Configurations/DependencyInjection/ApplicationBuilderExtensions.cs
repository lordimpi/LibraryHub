using LibraryHub.DataAccess.Context;
using LibraryHub.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LibraryHub.Configurations.DependencyInjection;

/// <summary>
/// Contiene extensiones para inicializacion de infraestructura al arrancar la aplicacion.
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Inicializa la base de datos de LibraryHub en modo desarrollo:
    /// aplica migraciones si existen y ejecuta el seeder si la base esta vacia.
    /// </summary>
    /// <param name="serviceProvider">Proveedor de servicios de la aplicacion.</param>
    /// <param name="isDevelopment">Indica si el entorno actual es desarrollo.</param>
    public static async Task InitializeLibraryHubDatabaseAsync(
        this IServiceProvider serviceProvider,
        bool isDevelopment)
    {
        if (!isDevelopment)
        {
            return;
        }

        await using var scope = serviceProvider.CreateAsyncScope();
        var scopedProvider = scope.ServiceProvider;
        var context = scopedProvider.GetRequiredService<LibraryHubDbContext>();
        var logger = scopedProvider.GetRequiredService<ILoggerFactory>()
            .CreateLogger("LibraryHub.DatabaseStartup");

        logger.LogInformation("Database initialization in development mode started.");

        var hasMigrations = context.Database.GetMigrations().Any();

        if (hasMigrations)
        {
            logger.LogInformation("Migrations detected. Applying pending migrations.");
            await context.Database.MigrateAsync();
        }
        else
        {
            logger.LogInformation("No migrations detected. Ensuring database is created.");
            await context.Database.EnsureCreatedAsync();
        }

        await DbInitializer.SeedAsync(context, logger);

        logger.LogInformation("Database initialization in development mode finished.");
    }
}
