using System;
using System.Collections.Generic;
#if NET5_0_OR_GREATER || NETCOREAPP3_0_OR_GREATER
using System.Runtime.InteropServices;
#endif

namespace Tedd
{
    public static class ToHashSetExtensions
    {
        #region Public

        #region No selector

        public static HashSet<TKey> ToHashSet<TKey>(this IEnumerable<TKey> source) =>
            ToHashSet(source, (IEqualityComparer<TKey>)null);

        public static HashSet<TKey> ToHashSet<TKey>(this IEnumerable<TKey> source, IEqualityComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var capacity = 0;
            if (source is ICollection<TKey> collection)
            {
                capacity = collection.Count;
                if (capacity == 0)
                    return new HashSet<TKey>(comparer);
            }

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER || NET8_0_OR_GREATER || NET10_0_OR_GREATER
            var d = new HashSet<TKey>(capacity, comparer);
#else
            var d = new HashSet<TKey>(comparer);
#endif

            if (source is ICollection<TKey> collection2)
            {
                if (collection2 is TKey[] array)
                {
#if NET5_0_OR_GREATER || NETCOREAPP3_0_OR_GREATER
                    var span = array.AsSpan();
                    foreach (ref readonly var item in span)
                        d.Add(item);
#else
                    for (var i = 0; i < array.Length; i++)
                        d.Add(array[i]);
#endif
                    return d;
                }

                if (collection2 is List<TKey> list)
                {
#if NET5_0_OR_GREATER || NETCOREAPP3_0_OR_GREATER
                    var span = CollectionsMarshal.AsSpan(list);
                    foreach (ref readonly var item in span)
                        d.Add(item);
#else
                    for (var i = 0; i < list.Count; i++)
                        d.Add(list[i]);
#endif
                    return d;
                }
            }

            foreach (var element in source)
                d.Add(element);

            return d;
        }
        #endregion

        #region KeySelector
        public static HashSet<TKey> ToHashSet<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) =>
            ToHashSet(source, keySelector, null);

        public static HashSet<TKey> ToHashSet<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            var capacity = 0;
            if (source is ICollection<TSource> collection)
            {
                capacity = collection.Count;
                if (capacity == 0)
                    return new HashSet<TKey>(comparer);
            }

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER || NET8_0_OR_GREATER || NET10_0_OR_GREATER
            var d = new HashSet<TKey>(capacity, comparer);
#else
            var d = new HashSet<TKey>(comparer);
#endif

            if (source is ICollection<TSource> collection2)
            {
                if (collection2 is TSource[] array)
                {
#if NET5_0_OR_GREATER || NETCOREAPP3_0_OR_GREATER
                    var span = array.AsSpan();
                    foreach (ref readonly var item in span)
                        d.Add(keySelector(item));
#else
                    for (var i = 0; i < array.Length; i++)
                        d.Add(keySelector(array[i]));
#endif
                    return d;
                }

                if (collection2 is List<TSource> list)
                {
#if NET5_0_OR_GREATER || NETCOREAPP3_0_OR_GREATER
                    var span = CollectionsMarshal.AsSpan(list);
                    foreach (ref readonly var item in span)
                        d.Add(keySelector(item));
#else
                    for (var i = 0; i < list.Count; i++)
                        d.Add(keySelector(list[i]));
#endif
                    return d;
                }
            }

            foreach (var element in source)
                d.Add(keySelector(element));

            return d;
        }
        #endregion
        #endregion
    }
}
