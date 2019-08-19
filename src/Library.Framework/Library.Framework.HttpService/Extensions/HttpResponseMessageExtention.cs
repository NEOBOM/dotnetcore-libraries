using Library.Framework.JsonUtil;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Library.HttpServices.Builders.Extensions
{
    public static class HttpResponseMessageExtention
    {
        private static string _regexMatch = @"([^\[]\w[^\]]*)|(\w)";

        public static T ExtractArrayToObjectSync<T>(this HttpResponseMessage httpResponseMessage) where T : class, new()
        {
            return ExtractArrayToObjectAsync<T>(httpResponseMessage).GetAwaiter().GetResult();
        }

        public static IList<T> ExtractArrayToListObjectSync<T>(this HttpResponseMessage httpResponseMessage) where T : class, new()
        {
            return ExtractArrayToListObjectAsync<T>(httpResponseMessage).GetAwaiter().GetResult();
        }

        public static async Task<T> ExtractArrayToObjectAsync<T>(this HttpResponseMessage httpResponseMessage) where T : class, new()
        {
            var content = await httpResponseMessage.Content.ReadAsStringAsync();

            var jarray = JsonHelper.JArrayParser(content);

            return CreateObject<T>(jarray);
        }

        public static async Task<IList<T>> ExtractArrayToListObjectAsync<T>(this HttpResponseMessage httpResponseMessage) where T : class, new()
        {
            var content = await httpResponseMessage.Content.ReadAsStringAsync();

            var jarray = JsonHelper.JArrayParser(content);

            var list = new List<T>(jarray.Count);

            foreach (var item in jarray)
            {
                list.Add(CreateObject<T>(item as JArray));
            }

            return list;
        }

        public static IList<T> ExtractContentArrayToListObjectSync<T>(this HttpResponseMessage httpResponseMessage) where T : class, new()
        {
            return ExtractContentArrayToListObjectAsync<T>(httpResponseMessage).GetAwaiter().GetResult();
        }

        public static async Task<IList<T>> ExtractContentArrayToListObjectAsync<T>(this HttpResponseMessage httpResponseMessage) where T : class, new()
        {
            var list = new List<T>();

            var content = await httpResponseMessage.Content.ReadAsStringAsync();

            var matches = Regex.Matches(content, _regexMatch, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

            foreach (Match item in matches)
            {
                if (item.Success)
                    list.Add(CreateObject<T>(item.Value.Replace(@"""", "").Split(",")));
            }

            return list;
        }

        public static T ExtractContentArrayToObjectSync<T>(this HttpResponseMessage httpResponseMessage) where T : class, new()
        {
            return ExtractContentArrayToObjectAsync<T>(httpResponseMessage).GetAwaiter().GetResult();
        }

        public static async Task<T> ExtractContentArrayToObjectAsync<T>(this HttpResponseMessage httpResponseMessage) where T : class, new()
        {
            var content = await httpResponseMessage.Content.ReadAsStringAsync();

            var match = Regex.Match(content, _regexMatch, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

            if (match.Success)
                return CreateObject<T>(match.Value.Replace(@"""", "").Split(","));

            return null;
        }

        private static T CreateObject<T>(string[] contents) where T : class, new()
        {
            T obj = new T();

            if (contents.Length > 0)
            {
                int index = 0;

                foreach (PropertyInfo property in obj.GetType().GetProperties())
                {
                    if (property.PropertyType == contents[index].GetType())
                        property.SetValue(obj, contents[index]);
                    else
                        property.SetValue(obj, Convert.ChangeType(contents[index], property.PropertyType));

                    index++;
                }
            }

            return obj;
        }

        private static T CreateObject<T>(JArray jArray, T obj = null) where T : class, new()
        {
            if (jArray.HasValues)
            {
                if(obj == null)
                    obj = new T();

                int index = 0;

                foreach (PropertyInfo property in obj.GetType().GetProperties())
                {
                    if(jArray[index].Type == JTokenType.Array)
                    {
                        CreateObject<T>(jArray[index] as JArray, obj);
                        continue;
                    }

                    if (property.PropertyType == jArray[index].GetType())
                        property.SetValue(obj, jArray[index]);
                    else
                        property.SetValue(obj, Convert.ChangeType(jArray[index], property.PropertyType));

                    index++;
                }
            }

            return obj;
        }
    }
}
