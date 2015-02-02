using System;
using System.Collections.Generic;
using System.Linq;

namespace Fync.Common
{
    public static class RecursionExtensions
    {
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