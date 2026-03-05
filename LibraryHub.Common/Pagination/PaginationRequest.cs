using System.ComponentModel.DataAnnotations;

namespace LibraryHub.Common.Pagination;

/// <summary>
/// Representa los parametros de entrada para una consulta paginada.
/// </summary>
public class PaginationRequest
{
    /// <summary>
    /// Obtiene o establece el numero de pagina solicitado.
    /// </summary>
    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Obtiene o establece la cantidad de elementos por pagina.
    /// </summary>
    [Range(1, 100)]
    public int PageSize { get; set; } = 5;

    /// <summary>
    /// Obtiene o establece la fuente de datos de paginacion.
    /// </summary>
    public PaginationSource Source { get; set; } = PaginationSource.Ef;

    /// <summary>
    /// Obtiene o establece el termino de busqueda opcional para filtrar resultados.
    /// </summary>
    [MaxLength(200)]
    public string? SearchTerm { get; set; }
}
