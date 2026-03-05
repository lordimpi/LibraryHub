using LibraryHub.Common.Configurations;
using LibraryHub.Common.IOptionPattern;
using Microsoft.Data.SqlClient;

namespace LibraryHub.DataAccess.Utilities.ADO;

/// <summary>
/// Define la fábrica de conexiones SQL para ejecución ADO.NET.
/// </summary>
public interface ISqlConnectionFactory
{
    /// <summary>
    /// Crea una nueva conexión SQL con la configuración actual.
    /// </summary>
    /// <returns>Instancia de <see cref="SqlConnection"/> lista para abrirse.</returns>
    SqlConnection Create();
}

/// <summary>
/// Implementa una fábrica de conexiones SQL basada en opciones monitoreadas.
/// </summary>
public sealed class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly IGenericOptionsService<DbOptions> _genericOptionsService;

    /// <summary>
    /// Inicializa una nueva instancia de la fábrica de conexiones.
    /// </summary>
    /// <param name="genericOptionsService">Servicio de opciones para resolver la cadena de conexión.</param>
    public SqlConnectionFactory(IGenericOptionsService<DbOptions> genericOptionsService)
    {
        _genericOptionsService = genericOptionsService;
    }

    /// <inheritdoc />
    public SqlConnection Create()
    {
        var connectionString = _genericOptionsService.GetMonitorOptions().DefaultConnection;
        return new SqlConnection(connectionString);
    }
}
