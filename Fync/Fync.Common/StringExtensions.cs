using System;
using System.Collections.Generic;

namespace Fync.Common
{
    public static class StringExtensions
    {
        public static string FormatWith(this string s, params object[] args)
        {
            return string.Format(s, args);
        }

        public static string StringJoin<T>(this IEnumerable<T> objects, string separator)
        {
            return string.Join(separator, objects);
        }

        public static string Unescape(this string s)
        {
            return Uri.UnescapeDataString(s);
        }
    }
}
