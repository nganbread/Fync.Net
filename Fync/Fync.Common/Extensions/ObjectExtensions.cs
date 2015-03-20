using System;
using System.Collections.Generic;
using System.Linq;

namespace Fync.Common
{
    public static class ObjectExtensions
    {
        public static TOut Map<TOut, TIn>(this TIn source, Func<TIn, TOut> mapper)
        {
            return source == null ? default(TOut) : mapper(source);
        }

        public static IList<TOut> MapToList<TOut, TIn>(this IEnumerable<TIn> source, Func<TIn, TOut> mapper)
        {
            return source.Select(mapper).ToList();
        }

        public static TOut SafeGet<TIn, TOut>(this TIn @this, Func<TIn, TOut> get, TOut @default = default(TOut))
        {
            return @this == null
                ? @default
                : get(@this);
        }

        public static IDictionary<string, object> ToDictionary(this object o)
        {
            return o
                .GetType()
                .GetProperties()
                .ToDictionary(x => x.Name, x => x.GetValue(o));
        } 
    }
}
