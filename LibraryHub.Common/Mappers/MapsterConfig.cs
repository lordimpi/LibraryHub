using LibraryHub.Common.DTOs;
using LibraryHub.Common.Entities;
using Mapster;

namespace LibraryHub.Common.Mappers;

/// <summary>
/// Centraliza la configuración de mapeos de Mapster para el dominio de LibraryHub.
/// </summary>
public static class MapsterConfig
{
    private static bool _configured;

    /// <summary>
    /// Registra los mapeos de Mapster una única vez durante el ciclo de vida de la aplicación.
    /// </summary>
    public static void RegisterMappings()
    {
        if (_configured)
        {
            return;
        }

        ConfigureAuthorMappings();
        ConfigureBookMappings();

        _configured = true;
    }

    private static void ConfigureAuthorMappings()
    {
        TypeAdapterConfig<CreateAuthorDto, AuthorEntity>
            .NewConfig();

        TypeAdapterConfig<AuthorEntity, AuthorDto>
            .NewConfig();
    }

    private static void ConfigureBookMappings()
    {
        TypeAdapterConfig<CreateBookDto, BookEntity>
            .NewConfig();

        TypeAdapterConfig<BookEntity, BookDto>
            .NewConfig()
            .Map(destination => destination.AuthorFullName,
                source => source.Author != null ? source.Author.FullName : string.Empty);
    }
}
