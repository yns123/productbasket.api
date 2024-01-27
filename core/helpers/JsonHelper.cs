using System.Text.Json;

namespace core.helpers;

public static class JsonHelper
{
    public static string ToJson(this object obj)
    {
        return JsonSerializer.Serialize(obj);
    }

    public static T FromJson<T>(this string str, JsonSerializerOptions? options = null)
    {
        if (String.IsNullOrWhiteSpace(str))
            return default!;

        return JsonSerializer.Deserialize<T>(str, options)!;
    }
}
