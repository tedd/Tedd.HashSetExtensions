using System;
using System.Collections.Generic;

namespace Tedd
{
    public static class ToHashSetSafeExtensions
    {
        #region Public
        #region KeySelector
        public static HashSet<TKey> ToHashSetSafe<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) =>
           ToHashSetSafe(source, keySelector, null);

        public static HashSet<TKey> ToHashSetSafe<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentException(nameof(source));

            if (keySelector == null)
                throw new ArgumentException(nameof(keySelector));

            var capacity = 0;
            if (source is ICollection<TSource> collection)
            {
                capacity = collection.Count;
                if (capacity == 0)
                    return new HashSet<TKey>(comparer);

                if (collection is TSource[] array)
                    return ToHashSetSafe(array, keySelector, comparer);

                if (collection is List<TSource> list)
                    return ToHashSetSafe(list, keySelector, comparer);
            }

            var d = new HashSet<TKey>(comparer);
            foreach (var element in source)
            {
                var ks = keySelector(element);
                if (!d.Contains(ks))
                    d.Add(ks);
            }

            return d;
        }
        #endregion
        #endregion


        #region Private
        #region Array
        private static HashSet<TKey> ToHashSetSafe<TSource, TKey>(TSource[] source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            var d = new HashSet<TKey>(comparer);
            for (var i = 0; i < source.Length; i++)
            {
                var ks = keySelector(source[i]);
                if (!d.Contains(ks))
                    d.Add(ks);
            }

            return d;
        }
        #endregion

        #region List
        private static HashSet<TKey> ToHashSetSafe<TSource, TKey>(List<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            var d = new HashSet<TKey>(comparer);
            foreach (TSource element in source)
            {
                var ks = keySelector(element);
                if (!d.Contains(ks))
                    d.Add(ks);
            }

            return d;
        }
        #endregion

        #endregion
    }
}
