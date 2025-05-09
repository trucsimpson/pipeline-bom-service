using System.Data.Common;

namespace BOMService.Infrastructure.Extensions
{
    public static class DbReaderExtensions
    {
        public static bool TryGetValue<T>(this DbDataReader reader, string columnName, out T value)
        {
            value = default!;
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    if (!reader.IsDBNull(i))
                    {
                        value = (T)reader.GetValue(i);
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }
    }
}
