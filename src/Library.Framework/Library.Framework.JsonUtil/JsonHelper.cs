using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Framework.JsonUtil
{
    public class JsonHelper
    {
        public static string SerializeObject(object content)
        {
            return JsonConvert.SerializeObject(content, Formatting.None);
        }

        public static T DeserializeObject<T>(string content) where T : class
        {
            return JsonConvert.DeserializeObject<T>(content);
        }

        public static object DeserializeObject(string content)
        {
            return JsonConvert.DeserializeObject(content);
        }

        public static JArray JArrayParser(string content)
        {
            return JArray.Parse(content);
        }
    }
}
