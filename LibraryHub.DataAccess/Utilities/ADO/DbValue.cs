namespace LibraryHub.DataAccess.Utilities.ADO;

/// <summary>
/// Provee conversiones comunes entre valores de dominio y valores de base de datos.
/// </summary>
public static class DbValue
{
    /// <summary>
    /// Convierte un valor a su representación para base de datos.
    /// </summary>
    /// <param name="value">Valor de origen.</param>
    /// <param name="treatZeroAsNull">Indica si cero debe interpretarse como <see cref="DBNull.Value"/>.</param>
    /// <returns>Valor apto para SQL o <see cref="DBNull.Value"/>.</returns>
    public static object ToDb(object? value, bool treatZeroAsNull = false)
    {
        if (value is null)
        {
            return DBNull.Value;
        }

        if (value is string text && string.IsNullOrWhiteSpace(text))
        {
            return DBNull.Value;
        }

        if (treatZeroAsNull && value is IConvertible convertible && Convert.ToDecimal(convertible) == 0m)
        {
            return DBNull.Value;
        }

        return value;
    }

    /// <summary>
    /// Convierte un valor de base de datos al tipo objetivo.
    /// </summary>
    /// <typeparam name="T">Tipo de salida.</typeparam>
    /// <param name="dbValue">Valor proveniente de base de datos.</param>
    /// <returns>Valor convertido al tipo solicitado.</returns>
    public static T FromDb<T>(object? dbValue)
    {
        if (dbValue is null || dbValue == DBNull.Value)
        {
            return default!;
        }

        var targetType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
        return (T)Convert.ChangeType(dbValue, targetType);
    }
}
