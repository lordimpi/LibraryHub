using LibraryHub.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryHub.DataAccess.Context;

/// <summary>
/// Define el contexto de base de datos principal para LibraryHub.
/// </summary>
public class LibraryHubDbContext : DbContext
{
    /// <summary>
    /// Inicializa una nueva instancia del contexto de LibraryHub.
    /// </summary>
    /// <param name="options">Opciones de configuracion del contexto.</param>
    public LibraryHubDbContext(DbContextOptions<LibraryHubDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Obtiene el conjunto de autores.
    /// </summary>
    public DbSet<AuthorEntity> Authors => Set<AuthorEntity>();

    /// <summary>
    /// Obtiene el conjunto de libros.
    /// </summary>
    public DbSet<BookEntity> Books => Set<BookEntity>();

    /// <summary>
    /// Configura el modelo de entidades aplicando configuraciones del ensamblado.
    /// </summary>
    /// <param name="modelBuilder">Constructor de modelos de EF Core.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LibraryHubDbContext).Assembly);

        modelBuilder.Entity<AuthorEntity>()
            .HasQueryFilter(author => !author.IsDeleted);

        modelBuilder.Entity<BookEntity>()
            .HasQueryFilter(book => !book.IsDeleted);
    }
}
