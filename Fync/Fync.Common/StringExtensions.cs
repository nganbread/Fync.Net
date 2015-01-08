using System.Collections.Generic;

namespace Fync.Common
{
    public static class EnumerableExtensions
    {
        public static void Add<T>(this ICollection<T> enumerable, params T[] add)
        {
            foreach (var item in add)
            {
                enumerable.Add(item);
            }
        }
    }
    public static class StringExtensions
    {
        public static string FormatWith(this string s, params object[] args)
        {
            return string.Format(s, args);
        }
    }
}
