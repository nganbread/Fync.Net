using System;
using System.Collections.Generic;
using System.Linq;

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

        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> enumerable, int splitSize)
        {
            return enumerable
                .Select((items, index) => new {items, group = index/splitSize})
                .GroupBy(x => x.group)
                .Select(group => group.Select(y => y.items));
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
            }

            return enumerable;
        }

        public static void Remove<T>(this ICollection<T> collection, IEnumerable<T> remove)
        {
            foreach (var item in remove.ToArray())
            {
                collection.Remove(item);
            }
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> enumerable, params T[] concat)
        {
            return Enumerable.Concat(enumerable, concat);
        }
    }
}