using Microsoft.Data.SqlClient;
using System.Data;

namespace LibraryHub.DataAccess.Utilities.ADO;

/// <summary>
/// Define operaciones base para ejecutar comandos SQL o procedimientos almacenados.
/// </summary>
public interface ISqlExecutor
{
    /// <summary>
    /// Ejecuta un comando sin retorno de filas.
    /// </summary>
    /// <param name="procOrSql">Nombre del procedimiento o texto SQL.</param>
    /// <param name="type">Tipo de comando.</param>
    /// <param name="parameters">Parámetros del comando.</param>
    /// <param name="tx">Transacción opcional.</param>
    /// <param name="timeout">Timeout opcional en segundos.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Cantidad de filas afectadas.</returns>
    Task<int> NonQueryAsync(
        string procOrSql,
        CommandType type,
        SqlParameter[]? parameters = null,
        SqlTransaction? tx = null,
        int? timeout = null,
        CancellationToken ct = default);

    /// <summary>
    /// Ejecuta un comando escalar.
    /// </summary>
    /// <param name="procOrSql">Nombre del procedimiento o texto SQL.</param>
    /// <param name="type">Tipo de comando.</param>
    /// <param name="parameters">Parámetros del comando.</param>
    /// <param name="tx">Transacción opcional.</param>
    /// <param name="timeout">Timeout opcional en segundos.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Valor escalar obtenido.</returns>
    Task<object?> ScalarAsync(
        string procOrSql,
        CommandType type,
        SqlParameter[]? parameters = null,
        SqlTransaction? tx = null,
        int? timeout = null,
        CancellationToken ct = default);

    /// <summary>
    /// Ejecuta un comando y mapea la lectura a una lista del tipo indicado.
    /// </summary>
    /// <typeparam name="T">Tipo de salida.</typeparam>
    /// <param name="procOrSql">Nombre del procedimiento o texto SQL.</param>
    /// <param name="type">Tipo de comando.</param>
    /// <param name="mapper">Función de mapeo del <see cref="IDataReader"/>.</param>
    /// <param name="parameters">Parámetros del comando.</param>
    /// <param name="tx">Transacción opcional.</param>
    /// <param name="timeout">Timeout opcional en segundos.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Lista con los resultados mapeados.</returns>
    Task<List<T>> QueryAsync<T>(
        string procOrSql,
        CommandType type,
        Func<IDataReader, List<T>> mapper,
        SqlParameter[]? parameters = null,
        SqlTransaction? tx = null,
        int? timeout = null,
        CancellationToken ct = default);
}

/// <summary>
/// Implementa operaciones ADO.NET para ejecutar SQL y procedimientos almacenados.
/// </summary>
public sealed class SqlExecutor : ISqlExecutor
{
    private readonly ISqlConnectionFactory _factory;
    private readonly int _defaultTimeout;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="SqlExecutor"/>.
    /// </summary>
    /// <param name="factory">Fábrica de conexiones SQL.</param>
    /// <param name="defaultTimeout">Timeout por defecto en segundos.</param>
    public SqlExecutor(ISqlConnectionFactory factory, int defaultTimeout = 30)
    {
        _factory = factory;
        _defaultTimeout = defaultTimeout;
    }

    /// <inheritdoc />
    public async Task<int> NonQueryAsync(
        string procOrSql,
        CommandType type,
        SqlParameter[]? parameters = null,
        SqlTransaction? tx = null,
        int? timeout = null,
        CancellationToken ct = default)
    {
        using var conn = tx?.Connection ?? _factory.Create();
        if (conn.State != ConnectionState.Open)
        {
            await conn.OpenAsync(ct);
        }

        using var cmd = new SqlCommand(procOrSql, conn, tx)
        {
            CommandType = type,
            CommandTimeout = timeout ?? _defaultTimeout,
        };

        if (parameters?.Length > 0)
        {
            cmd.Parameters.AddRange(parameters);
        }

        return await cmd.ExecuteNonQueryAsync(ct);
    }

    /// <inheritdoc />
    public async Task<object?> ScalarAsync(
        string procOrSql,
        CommandType type,
        SqlParameter[]? parameters = null,
        SqlTransaction? tx = null,
        int? timeout = null,
        CancellationToken ct = default)
    {
        using var conn = tx?.Connection ?? _factory.Create();
        if (conn.State != ConnectionState.Open)
        {
            await conn.OpenAsync(ct);
        }

        using var cmd = new SqlCommand(procOrSql, conn, tx)
        {
            CommandType = type,
            CommandTimeout = timeout ?? _defaultTimeout,
        };

        if (parameters?.Length > 0)
        {
            cmd.Parameters.AddRange(parameters);
        }

        return await cmd.ExecuteScalarAsync(ct);
    }

    /// <inheritdoc />
    public async Task<List<T>> QueryAsync<T>(
        string procOrSql,
        CommandType type,
        Func<IDataReader, List<T>> mapper,
        SqlParameter[]? parameters = null,
        SqlTransaction? tx = null,
        int? timeout = null,
        CancellationToken ct = default)
    {
        using var conn = tx?.Connection ?? _factory.Create();
        if (conn.State != ConnectionState.Open)
        {
            await conn.OpenAsync(ct);
        }

        using var cmd = new SqlCommand(procOrSql, conn, tx)
        {
            CommandType = type,
            CommandTimeout = timeout ?? _defaultTimeout,
        };

        if (parameters?.Length > 0)
        {
            cmd.Parameters.AddRange(parameters);
        }

        using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection, ct);
        return mapper(reader);
    }
}
