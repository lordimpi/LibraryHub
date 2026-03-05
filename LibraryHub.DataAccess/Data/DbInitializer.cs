using LibraryHub.Common.Entities;
using LibraryHub.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LibraryHub.DataAccess.Data;

/// <summary>
/// Inicializador de datos para poblar contenido base de LibraryHub.
/// </summary>
public static class DbInitializer
{
    /// <summary>
    /// Ejecuta el seeding inicial de autores y libros.
    /// Si ya existen datos, solo inserta los registros faltantes.
    /// </summary>
    /// <param name="context">Contexto de base de datos de LibraryHub.</param>
    /// <param name="logger">Logger opcional para registrar el estado del seeding.</param>
    public static async Task SeedAsync(LibraryHubDbContext context, ILogger? logger = null)
    {
        await context.Database.EnsureCreatedAsync();

        logger?.LogInformation("Seed started for LibraryHub.");

        var authorSeeds = new List<AuthorEntity>
        {
            new()
            {
                FullName = "Gabriel Garcia Marquez",
                BirthDate = new DateTime(1927, 3, 6),
                City = "Aracataca",
                Email = "gabo@libraryhub.local",
            },
            new()
            {
                FullName = "Isabel Allende",
                BirthDate = new DateTime(1942, 8, 2),
                City = "Lima",
                Email = "isabel@libraryhub.local",
            },
            new()
            {
                FullName = "Jorge Luis Borges",
                BirthDate = new DateTime(1899, 8, 24),
                City = "Buenos Aires",
                Email = "borges@libraryhub.local",
            },
        };

        var existingAuthors = await context.Authors.ToListAsync();
        var authorsByEmail = existingAuthors.ToDictionary(author => author.Email, StringComparer.OrdinalIgnoreCase);

        var authorsToInsert = authorSeeds
            .Where(author => !authorsByEmail.ContainsKey(author.Email))
            .ToList();

        if (authorsToInsert.Count > 0)
        {
            await context.Authors.AddRangeAsync(authorsToInsert);
            await context.SaveChangesAsync();

            foreach (var author in authorsToInsert)
            {
                authorsByEmail[author.Email] = author;
            }
        }

        var bookSeeds = new List<(string Title, int Year, string Genre, int Pages, string AuthorEmail)>
        {
            ("Cien anos de soledad", 1967, "Realismo magico", 417, "gabo@libraryhub.local"),
            ("El amor en los tiempos del colera", 1985, "Novela", 348, "gabo@libraryhub.local"),
            ("Cronica de una muerte anunciada", 1981, "Novela corta", 122, "gabo@libraryhub.local"),
            ("La casa de los espiritus", 1982, "Novela", 433, "isabel@libraryhub.local"),
            ("Eva Luna", 1987, "Novela", 304, "isabel@libraryhub.local"),
            ("Paula", 1994, "Memorias", 352, "isabel@libraryhub.local"),
            ("Ficciones", 1944, "Cuento", 224, "borges@libraryhub.local"),
            ("El Aleph", 1949, "Cuento", 146, "borges@libraryhub.local"),
            ("Historia universal de la infamia", 1935, "Relato", 176, "borges@libraryhub.local"),
        };

        var existingTitles = (await context.Books.Select(book => book.Title).ToListAsync())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var booksToInsert = new List<BookEntity>();

        foreach (var seed in bookSeeds)
        {
            if (existingTitles.Contains(seed.Title))
            {
                continue;
            }

            if (!authorsByEmail.TryGetValue(seed.AuthorEmail, out var author))
            {
                logger?.LogWarning(
                    "Book seed skipped because author was not found. Title: {Title}, AuthorEmail: {AuthorEmail}",
                    seed.Title,
                    seed.AuthorEmail);
                continue;
            }

            booksToInsert.Add(new BookEntity
            {
                Title = seed.Title,
                Year = seed.Year,
                Genre = seed.Genre,
                Pages = seed.Pages,
                AuthorId = author.Id,
            });
        }

        if (booksToInsert.Count > 0)
        {
            await context.Books.AddRangeAsync(booksToInsert);
            await context.SaveChangesAsync();
        }

        if (authorsToInsert.Count == 0 && booksToInsert.Count == 0)
        {
            logger?.LogInformation("Seed completed with no changes.");
            return;
        }

        logger?.LogInformation(
            "Seed finished for LibraryHub. AuthorsInserted: {AuthorsInserted}, BooksInserted: {BooksInserted}",
            authorsToInsert.Count,
            booksToInsert.Count);
    }
}
