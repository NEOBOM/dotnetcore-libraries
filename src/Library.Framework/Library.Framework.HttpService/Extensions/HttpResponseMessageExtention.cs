using Library.Framework.JsonUtil;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace Library.HttpServices.Builders.Extensions
{
    public static class HttpResponseMessageExtention
    {
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

            return JsonHelper.DeserializeObject<T>(content);
        }

        public static async Task<IList<T>> ExtractArrayToListObjectAsync<T>(this HttpResponseMessage httpResponseMessage) where T : class, new()
        {
            var content = await httpResponseMessage.Content.ReadAsStringAsync();

            return JsonHelper.DeserializeObject<IList<T>>(content);
        }
    }
}
