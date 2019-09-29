using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Framework.Commom
{
    public static class PoolFactory
    {
        public static StringBuilder StringBuilderAllocate()
        {
            var sb = DefaultNormalPool<StringBuilder>().Allocate();
            sb.Clear();

            return sb;
        }

        public static string StringBuilderReturnAndFree(StringBuilder builder)
        {
            DefaultNormalPool<StringBuilder>().ForgetTrackedObject(builder);
            return builder.ToString();
        }

        private static ObjectPool<T> DefaultNormalPool<T>() where T : class, new() =>  new ObjectPool<T>(() => new T(), 20);
    }
}
