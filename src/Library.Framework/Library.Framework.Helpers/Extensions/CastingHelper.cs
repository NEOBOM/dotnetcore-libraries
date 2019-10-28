using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Library.Framework.Helpers.Extensions
{
    public static class CastingHelper
    {
        private readonly static JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions { IgnoreNullValues = true };

        public static int AsInt(this string source)
        {
            if (string.IsNullOrEmpty(source)) throw new Exception("String is null or empty");

            var parsed = int.TryParse(source, out int value);

            if (!parsed) throw new Exception($"Invalid convert value {source} to int.");
            
            return value;
        }

        public static T DeserializeObjec<T>(this string source)
        {
            return JsonSerializer.Deserialize<T>(source);
        }

        public static string SerializeObject<T>(this T source)
        {
            return JsonSerializer.Serialize(source);
        }
    }
}
