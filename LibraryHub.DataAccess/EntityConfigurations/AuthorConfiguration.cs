using LibraryHub.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryHub.DataAccess.EntityConfigurations;

/// <summary>
/// Configura la entidad <see cref="AuthorEntity"/> para EF Core.
/// </summary>
public class AuthorConfiguration : IEntityTypeConfiguration<AuthorEntity>
{
    /// <summary>
    /// Configura el esquema de persistencia para la entidad de autor.
    /// </summary>
    /// <param name="builder">Constructor de configuracion de la entidad.</param>
    public void Configure(EntityTypeBuilder<AuthorEntity> builder)
    {
        builder.ToTable("Authors");

        builder.HasKey(author => author.Id);

        builder.Property(author => author.FullName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(author => author.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(author => author.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(author => author.IsDeleted)
            .HasDefaultValue(false);
    }
}
