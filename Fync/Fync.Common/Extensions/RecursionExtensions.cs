using System;
using System.Collections.Generic;
using System.Linq;

namespace Fync.Common
{
    public static class RecursionExtensions
    {
        public static void RecursivelyDo<T>(this T @this, Action<T> set, Func<T, IEnumerable<T>> recurse)
        {
            set(@this);
            foreach (var next in recurse(@this))
            {
                RecursivelyDo(next, set, recurse);
            }
        }

        public static IList<TGet> RecursivelyGetAll<T, TGet>(this T @this, Func<T, T> next, Func<T, TGet> get)
        {
            return PrivateRecursivelyGetAll(new List<TGet>(), @this, next, get);
        }

        private static IList<TGet> PrivateRecursivelyGetAll<T,TGet>(IList<TGet> objects, T @this, Func<T, T> next, Func<T, TGet> get)
        {
            if (@this == null) return objects;

            objects.Add(get(@this));
            var nextT = next(@this);
            return PrivateRecursivelyGetAll(objects, nextT, next, get);
        }
    }
}