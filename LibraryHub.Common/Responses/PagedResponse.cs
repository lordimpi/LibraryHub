namespace LibraryHub.Common.Responses;

/// <summary>
/// Representa una respuesta paginada estándar para la API.
/// </summary>
/// <typeparam name="T">Tipo de elemento contenido en la página.</typeparam>
public class PagedResponse<T>
{
    /// <summary>
    /// Inicializa una nueva instancia de <see cref="PagedResponse{T}"/>.
    /// </summary>
    /// <param name="items">Elementos de la página actual.</param>
    /// <param name="totalCount">Total de registros disponibles.</param>
    /// <param name="pageNumber">Número de página actual.</param>
    /// <param name="pageSize">Tamaño de página aplicado.</param>
    public PagedResponse(IReadOnlyCollection<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    /// <summary>
    /// Obtiene los elementos de la página actual.
    /// </summary>
    public IReadOnlyCollection<T> Items { get; }

    /// <summary>
    /// Obtiene el total de registros disponibles.
    /// </summary>
    public int TotalCount { get; }

    /// <summary>
    /// Obtiene el número de página actual.
    /// </summary>
    public int PageNumber { get; }

    /// <summary>
    /// Obtiene el tamaño de página aplicado.
    /// </summary>
    public int PageSize { get; }

    /// <summary>
    /// Obtiene el total de páginas calculado.
    /// </summary>
    public int TotalPages => TotalCount == 0 ? 0 : (int)Math.Ceiling((double)TotalCount / PageSize);

    /// <summary>
    /// Obtiene un valor que indica si existe una página anterior.
    /// </summary>
    public bool HasPreviousPage => PageNumber > 1;

    /// <summary>
    /// Obtiene un valor que indica si existe una página siguiente.
    /// </summary>
    public bool HasNextPage => PageNumber < TotalPages;
}
