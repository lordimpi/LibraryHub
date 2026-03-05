using LibraryHub.Bussiness.Interfaces;
using LibraryHub.Bussiness.Services;
using LibraryHub.Common.Configurations;
using LibraryHub.Common.IOptionPattern;
using LibraryHub.Common.Mappers;
using LibraryHub.Common.Options;
using LibraryHub.Configurations.Infrastructure.Abstractions;
using LibraryHub.Configurations.Infrastructure.Implementations;
using LibraryHub.DataAccess.Context;
using LibraryHub.DataAccess.Repositories.Implementations;
using LibraryHub.DataAccess.Repositories.Interfaces;
using LibraryHub.DataAccess.UnitOfWork;
using LibraryHub.DataAccess.Utilities.ADO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LibraryHub.Configurations.DependencyInjection;

/// <summary>
/// Contiene extensiones para registrar dependencias de LibraryHub.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registra opciones de configuración de la aplicación.
    /// </summary>
    /// <param name="services">Colección de servicios.</param>
    /// <param name="config">Configuración de la aplicación.</param>
    /// <returns>Colección de servicios actualizada.</returns>
    public static IServiceCollection AddLibraryHubOptions(this IServiceCollection services, IConfiguration config)
    {
        services.AddOptions();
        services.BindOptions<BusinessRulesOptions>(config, "BusinessRules");
        services.BindOptions<DbOptions>(config, "ConnectionStrings", validateDataAnnotations: false, validateOnStart: false);
        services.AddScoped(typeof(IGenericOptionsService<>), typeof(GenericOptionsService<>));

        return services;
    }

    /// <summary>
    /// Registra componentes de acceso a datos para LibraryHub.
    /// </summary>
    /// <param name="services">Colección de servicios.</param>
    /// <param name="config">Configuración de la aplicación.</param>
    /// <returns>Colección de servicios actualizada.</returns>
    /// <exception cref="InvalidOperationException">Se lanza cuando no existe la cadena de conexión <c>DefaultConnection</c>.</exception>
    public static IServiceCollection AddLibraryHubDataAccess(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<LibraryHubDbContext>((serviceProvider, options) =>
        {
            var dbOptions = serviceProvider
                .GetRequiredService<IGenericOptionsService<DbOptions>>()
                .GetMonitorOptions();

            if (string.IsNullOrWhiteSpace(dbOptions.DefaultConnection))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");
            }

            options.UseSqlServer(dbOptions.DefaultConnection);
        });

        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<ILibraryHubUnitOfWork, LibraryHubUnitOfWork>();
        services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();
        services.AddScoped<ISqlExecutor, SqlExecutor>();

        return services;
    }

    /// <summary>
    /// Registra servicios de negocio y componentes transversales.
    /// </summary>
    /// <param name="services">Colección de servicios.</param>
    /// <returns>Colección de servicios actualizada.</returns>
    public static IServiceCollection AddLibraryHubBussiness(this IServiceCollection services)
    {
        MapsterConfig.RegisterMappings();

        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<IBookService, BookService>();

        // Registro transient para demostrar el patrón de lifetimes.
        services.AddTransient<IClock, SystemClock>();

        return services;
    }

    /// <summary>
    /// Registra toda la configuración de LibraryHub en un único punto.
    /// </summary>
    /// <param name="services">Colección de servicios.</param>
    /// <param name="config">Configuración de la aplicación.</param>
    /// <returns>Colección de servicios actualizada.</returns>
    public static IServiceCollection AddLibraryHub(this IServiceCollection services, IConfiguration config)
    {
        services.AddLibraryHubOptions(config);
        services.AddLibraryHubDataAccess(config);
        services.AddLibraryHubBussiness();

        return services;
    }

    /// <summary>
    /// Vincula una clase de configuración a una sección del <see cref="IConfiguration"/>.
    /// </summary>
    /// <typeparam name="TOptions">Tipo de opciones a registrar.</typeparam>
    /// <param name="services">Colección de servicios.</param>
    /// <param name="configuration">Configuración de la aplicación.</param>
    /// <param name="sectionName">Nombre de la sección a vincular.</param>
    /// <param name="validateDataAnnotations">Indica si se validan DataAnnotations.</param>
    /// <param name="validateOnStart">Indica si la validación se ejecuta en startup.</param>
    /// <returns>Builder de opciones para encadenar validaciones adicionales.</returns>
    public static OptionsBuilder<TOptions> BindOptions<TOptions>(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName,
        bool validateDataAnnotations = true,
        bool validateOnStart = true)
        where TOptions : class, new()
    {
        var builder = services
            .AddOptions<TOptions>()
            .Bind(configuration.GetSection(sectionName));

        if (validateDataAnnotations)
        {
            builder.ValidateDataAnnotations();
        }

        if (validateOnStart)
        {
            builder.ValidateOnStart();
        }

        return builder;
    }
}
