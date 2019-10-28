using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Framework.Helpers.Extensions
{
    /// <summary>
    /// Extension for List
    /// </summary>
    public static class ListExtensionHelper
    {
        /// <summary>
        /// Verify this this list contains item
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="list">This</param>
        /// <returns></returns>
        public static bool ContainItem<T>(this List<T> list)
        {
            return (list != null && list.Any());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="list">This</param>
        /// <param name="content"></param>
        public static void AddItem<T>(this List<T> list, T content)
        {
            if (content != null && list != null)
                list.Add(content);
        }

        /// <summary>
        /// Add items
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="list">This</param>
        /// <param name="contents">Generict List</param>
        public static void AddItems<T>(this List<T> list, List<T> contents)
        {
            if (list != null && contents.ContainItem())
                list.AddRange(contents);
        }

        /// <summary>
        /// Chuck the list
        /// </summary>
        /// <typeparam name="T">Generic type</typeparam>
        /// <param name="source">This</param>
        /// <param name="chunkSize">Chunk size</param>
        /// <returns></returns>
        public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
    }
}
