using LibraryHub.Configurations.Infrastructure.Abstractions;

namespace LibraryHub.Configurations.Infrastructure.Implementations;

/// <summary>
/// Implementa un reloj basado en la hora del sistema.
/// </summary>
public class SystemClock : IClock
{
    /// <inheritdoc />
    public DateTime UtcNow => DateTime.UtcNow;
}
