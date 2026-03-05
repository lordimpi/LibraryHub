namespace LibraryHub.Common.Pagination;

/// <summary>
/// Representa el resultado de una consulta paginada en la capa de datos.
/// </summary>
/// <typeparam name="T">Tipo de elemento devuelto.</typeparam>
public class PagedQueryResult<T>
{
    /// <summary>
    /// Inicializa una nueva instancia de <see cref="PagedQueryResult{T}"/>.
    /// </summary>
    /// <param name="items">Colección de elementos paginados.</param>
    /// <param name="totalCount">Total de registros disponibles sin paginar.</param>
    public PagedQueryResult(IReadOnlyCollection<T> items, int totalCount)
    {
        Items = items;
        TotalCount = totalCount;
    }

    /// <summary>
    /// Obtiene la colección de elementos de la página actual.
    /// </summary>
    public IReadOnlyCollection<T> Items { get; }

    /// <summary>
    /// Obtiene el total de registros sin aplicar paginación.
    /// </summary>
    public int TotalCount { get; }
}
