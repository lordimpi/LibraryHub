using Microsoft.Data.SqlClient;
using System.Data;

namespace LibraryHub.DataAccess.Utilities.ADO;

/// <summary>
/// Provee helpers para crear parámetros de SQL Server.
/// </summary>
public static class SqlParam
{
    /// <summary>
    /// Crea un parámetro de entrada.
    /// </summary>
    /// <param name="name">Nombre del parámetro sin prefijo <c>@</c>.</param>
    /// <param name="type">Tipo SQL del parámetro.</param>
    /// <param name="value">Valor del parámetro.</param>
    /// <returns>Instancia configurada de <see cref="SqlParameter"/>.</returns>
    public static SqlParameter In(string name, SqlDbType type, object? value)
    {
        return new SqlParameter($"@{name}", type) { Value = value ?? DBNull.Value };
    }

    /// <summary>
    /// Crea un parámetro de salida.
    /// </summary>
    /// <param name="name">Nombre del parámetro sin prefijo <c>@</c>.</param>
    /// <param name="type">Tipo SQL del parámetro.</param>
    /// <param name="size">Tamaño opcional para tipos variables.</param>
    /// <returns>Instancia configurada de <see cref="SqlParameter"/>.</returns>
    public static SqlParameter Out(string name, SqlDbType type, int size = 0)
    {
        var parameter = new SqlParameter($"@{name}", type)
        {
            Direction = ParameterDirection.Output,
        };

        if (size > 0)
        {
            parameter.Size = size;
        }

        return parameter;
    }
}
