using LibraryHub.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryHub.DataAccess.EntityConfigurations;

/// <summary>
/// Configura la entidad <see cref="BookEntity"/> para EF Core.
/// </summary>
public class BookConfiguration : IEntityTypeConfiguration<BookEntity>
{
    /// <summary>
    /// Configura el esquema de persistencia para la entidad de libro.
    /// </summary>
    /// <param name="builder">Constructor de configuracion de la entidad.</param>
    public void Configure(EntityTypeBuilder<BookEntity> builder)
    {
        builder.ToTable("Books");

        builder.HasKey(book => book.Id);

        builder.Property(book => book.Title)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(book => book.Genre)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(book => book.IsDeleted)
            .HasDefaultValue(false);

        builder.HasOne(book => book.Author)
            .WithMany()
            .HasForeignKey(book => book.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
