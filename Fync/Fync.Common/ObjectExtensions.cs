using System;
using System.Collections.Generic;
using System.Linq;

namespace Fync.Common
{
    public static class ObjectExtensions
    {
        public static TOut Map<TOut, TIn>(this TIn source, Func<TIn, TOut> mapper)
        {
            return mapper(source);
        }

        public static IList<TOut> MapToList<TOut, TIn>(this IEnumerable<TIn> source, Func<TIn, TOut> mapper)
        {
            return source.Select(mapper).ToList();
        }

        public static void RecursivelySet<T>(this T @this, Action<T> set, Func<T, IEnumerable<T>> recurse)
        {
            set(@this);
            foreach (var next in recurse(@this))
            {
                RecursivelySet(next, set, recurse);
            }
        }
    }
}
