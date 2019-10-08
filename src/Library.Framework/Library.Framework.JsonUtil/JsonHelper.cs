using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Library.Framework.JsonUtil
{
    public class JsonHelper
    {
        private readonly static JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions { IgnoreNullValues = true };

        public static string SerializeObject(object content)
        {
            return JsonSerializer.Serialize(content, _jsonSerializerOptions);
        }

        public static T DeserializeObject<T>(string content) where T : class
        {
            return JsonSerializer.Deserialize<T>(content, _jsonSerializerOptions);
        }

        public static object DeserializeObject(string content)
        {
            return JsonSerializer.Deserialize(content, typeof(object), _jsonSerializerOptions);
        }
    }
}
