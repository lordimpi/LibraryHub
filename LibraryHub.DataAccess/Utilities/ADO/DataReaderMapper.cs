using System.Data;
using System.Reflection;

namespace LibraryHub.DataAccess.Utilities.ADO;

/// <summary>
/// Provee mapeo reflexivo de registros ADO.NET hacia objetos.
/// </summary>
public static class DataReaderMapper
{
    /// <summary>
    /// Mapea un <see cref="IDataReader"/> a una lista del tipo indicado.
    /// </summary>
    /// <typeparam name="T">Tipo de salida a poblar.</typeparam>
    /// <param name="dataReader">Reader con el resultado a mapear.</param>
    /// <returns>Lista de instancias con los datos leídos.</returns>
    public static List<T> MapToList<T>(IDataReader dataReader)
        where T : new()
    {
        var result = new List<T>();
        var properties = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(property => property.CanWrite)
            .ToArray();

        var ordinals = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        for (var index = 0; index < dataReader.FieldCount; index++)
        {
            ordinals[dataReader.GetName(index)] = index;
        }

        while (dataReader.Read())
        {
            var item = new T();

            foreach (var property in properties)
            {
                if (!ordinals.TryGetValue(property.Name, out var ordinal))
                {
                    continue;
                }

                var value = dataReader.GetValue(ordinal);
                if (value == DBNull.Value)
                {
                    property.SetValue(item, null);
                    continue;
                }

                var targetType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                property.SetValue(item, Convert.ChangeType(value, targetType));
            }

            result.Add(item);
        }

        return result;
    }
}
