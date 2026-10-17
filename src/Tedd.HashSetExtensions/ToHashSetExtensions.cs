using System;
using System.Collections.Generic;

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
            if (source == null)
                throw new ArgumentException(nameof(source));

            if (source is ICollection<TKey> collection)
            {
                if (collection.Count == 0)
                    return new HashSet<TKey>(comparer);

                if (collection is TKey[] array)
                {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
                    var d = new HashSet<TKey>(array.Length, comparer);
#else
                    var d = new HashSet<TKey>(comparer);
#endif
                    for (var i = 0; i < array.Length; i++)
                        d.Add(array[i]);
                    return d;
                }

                if (collection is List<TKey> list)
                {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
                    var d = new HashSet<TKey>(list.Count, comparer);
#else
                    var d = new HashSet<TKey>(comparer);
#endif
#if NET5_0_OR_GREATER
                    foreach (var item in System.Runtime.InteropServices.CollectionsMarshal.AsSpan(list))
                        d.Add(item);
#else
                    for (var i = 0; i < list.Count; i++)
                        d.Add(list[i]);
#endif
                    return d;
                }

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
                var collectionSet = new HashSet<TKey>(collection.Count, comparer);
#else
                var collectionSet = new HashSet<TKey>(comparer);
#endif
                foreach (var element in collection)
                    collectionSet.Add(element);
                return collectionSet;
            }

            var defaultSet = new HashSet<TKey>(comparer);
            foreach (var element in source)
                defaultSet.Add(element);

            return defaultSet;
        }
        #endregion

        #region KeySelector
        public static HashSet<TKey> ToHashSet<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) =>
            ToHashSet(source, keySelector, null);

        public static HashSet<TKey> ToHashSet<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentException(nameof(source));

            if (keySelector == null)
                throw new ArgumentException(nameof(keySelector));

            if (source is ICollection<TSource> collection)
            {
                var capacity = collection.Count;
                if (capacity == 0)
                    return new HashSet<TKey>(comparer);

                if (collection is TSource[] array)
                    return ToHashSet(array, keySelector, comparer);

                if (collection is List<TSource> list)
                    return ToHashSet(list, keySelector, comparer);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
                var collectionSet = new HashSet<TKey>(capacity, comparer);
#else
                var collectionSet = new HashSet<TKey>(comparer);
#endif
                foreach (var element in collection)
                    collectionSet.Add(keySelector(element));
                return collectionSet;
            }

            var d = new HashSet<TKey>(comparer);
            foreach (var element in source)
                d.Add(keySelector(element));

            return d;
        }
        #endregion
        #endregion


        #region Private
        #region Array
        private static HashSet<TKey> ToHashSet<TSource, TKey>(TSource[] source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
            var d = new HashSet<TKey>(source.Length, comparer);
#else
            var d = new HashSet<TKey>(comparer);
#endif
            for (var i = 0; i < source.Length; i++)
                d.Add(keySelector(source[i]));

            return d;
        }
        #endregion

        #region List
        private static HashSet<TKey> ToHashSet<TSource, TKey>(List<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
            var d = new HashSet<TKey>(source.Count, comparer);
#else
            var d = new HashSet<TKey>(comparer);
#endif
#if NET5_0_OR_GREATER
            foreach (var item in System.Runtime.InteropServices.CollectionsMarshal.AsSpan(source))
                d.Add(keySelector(item));
#else
            // Kept original loop but optimized to for-loop since List<T> supports O(1) indexer
            for (var i = 0; i < source.Count; i++)
                d.Add(keySelector(source[i]));
#endif

            return d;
        }
        #endregion


        #endregion
    }
}