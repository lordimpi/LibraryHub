namespace LibraryHub.Configurations.Infrastructure.Abstractions;

/// <summary>
/// Define un proveedor de fecha y hora en UTC.
/// </summary>
public interface IClock
{
    /// <summary>
    /// Obtiene la fecha y hora actual en formato UTC.
    /// </summary>
    DateTime UtcNow { get; }
}
