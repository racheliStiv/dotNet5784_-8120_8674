using System.Reflection;

namespace BO;
internal static class Tools
{
    public static string ToStringProperty<T>(this T entity)
    {
        Type type = typeof(T);
        PropertyInfo[] properties = type.GetProperties();

        List<string> propertyValues = new List<string>();

        foreach (PropertyInfo property in properties)
        {
            object? value = property.GetValue(entity);
            if (value != null)
            {
                if (value is IEnumerable<object>)
                {
                    // If property is a collection, concatenate its elements
                    var collectionValues = new List<string>();
                    foreach (var item in (IEnumerable<object>)value)
                    {
                        if (item != null)
                            collectionValues.Add(item.ToString()!);
                    }
                    propertyValues.Add($"{property.Name}: [{string.Join(", ", collectionValues)}]");
                }
                else
                {
                    propertyValues.Add($"{property.Name}: {value}");
                }
            }
        }
        return string.Join(", ", propertyValues);
    }
}
