using System;
using System.Collections.Generic;
using Nokia3310.Applications.Common;

namespace Nokia3310.Applications.Extensions
{
    public static class CollectionExtensions
    {
        public static T Random<T>(this IReadOnlyList<T> list)
        {
            if (list.Count == 0)
            {
                throw new ArgumentException("Collection is empty", nameof(list));
            }

            return list[NokiaApp.Random.Next(0, list.Count)];
        }

        public static bool HasIndex<T>(this IReadOnlyList<T> list, int index)
        {
            return index >= 0 && index < list.Count;
        }

        public static T GetIndexOrLast<T>(this IReadOnlyList<T> list, int index)
        {
            if (list.Count == 0)
            {
                throw new ArgumentException("Collection is empty", nameof(list));
            }

            if (index >= 0 && index < list.Count)
            {
                return list[index];
            }

            return list[list.Count - 1];
        }

        public static void AddNotNull<T>(this ICollection<T> list, T value)
        {
            if (value != null)
            {
                list.Add(value);
            }
        }
    }
}
