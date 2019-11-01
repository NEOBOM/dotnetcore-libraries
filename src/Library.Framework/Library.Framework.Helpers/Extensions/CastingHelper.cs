using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Library.Framework.Helpers.Extensions
{
    public static class CastingHelper
    {
        private readonly static JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions 
        { 
            IgnoreNullValues = true, 
            IgnoreReadOnlyProperties = true 
        };

        public static int ToTryInt(this string source)
        {
            if (string.IsNullOrEmpty(source)) throw new Exception("String is null or empty");

            var parsed = int.TryParse(source, out int value);

            if (!parsed) throw new Exception($"Invalid convert value {source} to int.");
            
            return value;
        }

        public static T DeserializeObject<T>(string content) where T : class
        {
            return JsonSerializer.Deserialize<T>(content, _jsonSerializerOptions);
        }

        public static object DeserializeObject(string content)
        {
            return JsonSerializer.Deserialize(content, typeof(object), _jsonSerializerOptions);
        }

        public static string SerializeObject<T>(this T source)
        {
            return JsonSerializer.Serialize(source, _jsonSerializerOptions);
        }
    }
}
