using System;
using System.Collections.Generic;

namespace Tedd
{
    public static class ContainsExtensions
    {
        public static bool Contains<TKey>(this HashSet<TKey> hashSet, IEnumerable<TKey> values)
        {
            if (values == null)
                throw new ArgumentException(nameof(values));

            if (values is ICollection<TKey> collection)
            {
                if (collection.Count == 0)
                    return false;

                if (collection is TKey[] array)
                {
                    for (var i = 0; i < array.Length; i++)
                        if (hashSet.Contains(array[i]))
                            return true;
                    return false;
                }

                if (collection is List<TKey> list)
                {
                    for (var i = 0; i < list.Count; i++)
                        if (hashSet.Contains(list[i]))
                            return true;
                    return false;
                }
            }

            foreach (var element in values)
                if (hashSet.Contains(element))
                    return true;

            return false;
        }

    }
}
