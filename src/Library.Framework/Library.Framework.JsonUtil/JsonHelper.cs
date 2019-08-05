using Newtonsoft.Json;
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

        public static T Deserealize<T>(string content) where T : class
        {
            return JsonConvert.DeserializeObject<T>(content);
        }

        public static object Deserealize(string content)
        {
            return JsonConvert.DeserializeObject(content);
        }
    }
}
