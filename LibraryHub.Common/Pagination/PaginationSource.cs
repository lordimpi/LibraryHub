namespace LibraryHub.Common.Pagination;

/// <summary>
/// Define las estrategias disponibles para obtener datos paginados.
/// </summary>
public enum PaginationSource
{
    /// <summary>
    /// Usa EF Core con paginación por Skip/Take.
    /// </summary>
    Ef = 0,

    /// <summary>
    /// Usa procedimientos almacenados mediante ADO.NET.
    /// </summary>
    Sp = 1,
}
