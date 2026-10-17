using System;
using System.Collections.Generic;

namespace Tedd
{
    public static class AddRemoveRangeExtensions
    {
        #region AddRange
        #region No selector
        public static int AddRange<TKey>(this HashSet<TKey> hashSet, IEnumerable<TKey> values)
        {
            var count = 0;

            if (values == null)
                throw new ArgumentException(nameof(values));

            if (values is ICollection<TKey> collection)
            {
                if (collection.Count == 0)
                    return 0;

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
                hashSet.EnsureCapacity(hashSet.Count + collection.Count);
#endif

                if (collection is TKey[] array)
                {
                    for (var i = 0; i < array.Length; i++)
                        if (hashSet.Add(array[i]))
                            count++;
                    return count;
                }

                if (collection is List<TKey> list)
                {
#if NET5_0_OR_GREATER
                    foreach (var item in System.Runtime.InteropServices.CollectionsMarshal.AsSpan(list))
                        if (hashSet.Add(item))
                            count++;
#else
                    for (var i = 0; i < list.Count; i++)
                        if (hashSet.Add(list[i]))
                            count++;
#endif
                    return count;
                }
            }

            foreach (var element in values)
                if (hashSet.Add(element))
                    count++;
            return count;
        }
        #endregion

        #region Selector
        public static int AddRange<TSource, TKey>(this HashSet<TKey> hashSet, IEnumerable<TSource> values, Func<TSource, TKey> keySelector)
        {
            var count = 0;

            if (values == null)
                throw new ArgumentException(nameof(values));

            if (values is ICollection<TSource> collection)
            {
                if (collection.Count == 0)
                    return 0;

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
                hashSet.EnsureCapacity(hashSet.Count + collection.Count);
#endif

                if (collection is TSource[] array)
                {
                    for (var i = 0; i < array.Length; i++)
                        if (hashSet.Add(keySelector(array[i])))
                            count++;
                    return count;
                }

                if (collection is List<TSource> list)
                {
#if NET5_0_OR_GREATER
                    foreach (var item in System.Runtime.InteropServices.CollectionsMarshal.AsSpan(list))
                        if (hashSet.Add(keySelector(item)))
                            count++;
#else
                    for (var i = 0; i < list.Count; i++)
                        if (hashSet.Add(keySelector(list[i])))
                            count++;
#endif
                    return count;
                }
            }

            foreach (var element in values)
                if (hashSet.Add(keySelector(element)))
                    count++;
            return count;
        }
        #endregion
        #endregion

        #region RemoveRange
        #region No selector
        public static int RemoveRange<TKey>(this HashSet<TKey> hashSet, IEnumerable<TKey> values)
        {
            var count = 0;

            if (values == null)
                throw new ArgumentException(nameof(values));

            if (values is ICollection<TKey> collection)
            {
                if (collection.Count == 0)
                    return 0;

                if (collection is TKey[] array)
                {
                    for (var i = 0; i < array.Length; i++)
                        if (hashSet.Remove(array[i]))
                            count++;
                    return count;
                }

                if (collection is List<TKey> list)
                {
#if NET5_0_OR_GREATER
                    foreach (var item in System.Runtime.InteropServices.CollectionsMarshal.AsSpan(list))
                        if (hashSet.Remove(item))
                            count++;
#else
                    for (var i = 0; i < list.Count; i++)
                        if (hashSet.Remove(list[i]))
                            count++;
#endif
                    return count;
                }

            }

            foreach (var element in values)
                if (hashSet.Remove(element))
                    count++;
            return count;
        }
        #endregion

        #region Selector
        public static int RemoveRange<TSource, TKey>(this HashSet<TKey> hashSet, IEnumerable<TSource> values, Func<TSource, TKey> keySelector)
        {
            var count = 0;

            if (values == null)
                throw new ArgumentException(nameof(values));

            if (values is ICollection<TSource> collection)
            {
                if (collection.Count == 0)
                    return 0;

                if (collection is TSource[] array)
                {
                    for (var i = 0; i < array.Length; i++)
                        if (hashSet.Remove(keySelector(array[i])))
                            count++;
                    return count;
                }

                if (collection is List<TSource> list)
                {
#if NET5_0_OR_GREATER
                    foreach (var item in System.Runtime.InteropServices.CollectionsMarshal.AsSpan(list))
                        if (hashSet.Remove(keySelector(item)))
                            count++;
#else
                    for (var i = 0; i < list.Count; i++)
                        if (hashSet.Remove(keySelector(list[i])))
                            count++;
#endif
                    return count;
                }

            }

            foreach (var element in values)
                if (hashSet.Remove(keySelector(element)))
                    count++;
            return count;
        }
        #endregion
        #endregion

    }
}