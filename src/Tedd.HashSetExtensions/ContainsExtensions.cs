using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Tedd
{
    public static class ContainsRangeExtensions
    {
        public static bool ContainsRange<TKey>(this HashSet<TKey> hashSet, IEnumerable<TKey> values)
        {
            if (values == null)
                throw new ArgumentException(nameof(values));

            if (values is ICollection<TKey> collection)
            {
                if (collection.Count == 0 || hashSet.Count == 0)
                    return false;

                if (collection is TKey[] array)
                {
#if NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
                    // Time Complexity: O(n) where n is the length of the array
                    // Space Complexity: O(1) supplementary memory allocations, leveraging Span slice.
                    foreach (var item in array.AsSpan())
                        if (hashSet.Contains(item))
                            return true;
#else
                    for (var i = 0; i < array.Length; i++)
                        if (hashSet.Contains(array[i]))
                            return true;
#endif
                    return false;
                }

                if (collection is List<TKey> list)
                {
#if NET5_0_OR_GREATER
                    // Time Complexity: O(n) where n is the count of the list
                    // Space Complexity: O(1) supplementary memory allocations, leveraging CollectionsMarshal.AsSpan.
                    foreach (var item in CollectionsMarshal.AsSpan(list))
                        if (hashSet.Contains(item))
                            return true;
#else
                    for (var i = 0; i < list.Count; i++)
                        if (hashSet.Contains(list[i]))
                            return true;
#endif
                    return false;
                }
            }

            foreach (var element in values)
                if (hashSet.Contains(element))
                    return true;

            return false;
        }

        public static bool ContainsRange<TSource, TKey>(this HashSet<TKey> hashSet, IEnumerable<TSource> values, Func<TSource, TKey> keySelector)
        {
            if (values == null)
                throw new ArgumentException(nameof(values));

            if (values is ICollection<TSource> collection)
            {
                if (collection.Count == 0 || hashSet.Count == 0)
                    return false;

                if (collection is TSource[] array)
                {
#if NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
                    // Time Complexity: O(n) where n is the length of the array
                    // Space Complexity: O(1) supplementary memory allocations, leveraging Span slice.
                    foreach (var item in array.AsSpan())
                        if (hashSet.Contains(keySelector(item)))
                            return true;
#else
                    for (var i = 0; i < array.Length; i++)
                        if (hashSet.Contains(keySelector(array[i])))
                            return true;
#endif
                    return false;
                }

                if (collection is List<TSource> list)
                {
#if NET5_0_OR_GREATER
                    // Time Complexity: O(n) where n is the count of the list
                    // Space Complexity: O(1) supplementary memory allocations, leveraging CollectionsMarshal.AsSpan.
                    foreach (var item in CollectionsMarshal.AsSpan(list))
                        if (hashSet.Contains(keySelector(item)))
                            return true;
#else
                    for (var i = 0; i < list.Count; i++)
                        if (hashSet.Contains(keySelector(list[i])))
                            return true;
#endif
                    return false;
                }
            }

            foreach (var element in values)
                if (hashSet.Contains(keySelector(element)))
                    return true;

            return false;
        }

    }
}
