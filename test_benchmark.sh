cat << 'INNER_EOF' > src/Tedd.HashSetExtensions/AddRemoveRangeExtensions.cs
using System;
using System.Collections.Generic;
#if NET5_0_OR_GREATER || NETCOREAPP3_0_OR_GREATER
using System.Runtime.InteropServices;
#endif

namespace Tedd
{
    public static class AddRemoveRangeExtensions
    {
        public static int AddRange<TKey>(this HashSet<TKey> hashSet, IEnumerable<TKey> values)
        {
            if (hashSet == null) throw new ArgumentNullException(nameof(hashSet));
            if (values == null) throw new ArgumentNullException(nameof(values));

            var count = 0;

            if (values is ICollection<TKey> collection)
            {
                if (collection.Count == 0)
                    return 0;

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER || NET8_0_OR_GREATER || NET10_0_OR_GREATER
                hashSet.EnsureCapacity(hashSet.Count + collection.Count);
#endif

                if (collection is TKey[] array)
                {
#if NET5_0_OR_GREATER || NETCOREAPP3_0_OR_GREATER
                    var span = array.AsSpan();
                    foreach (ref readonly var item in span)
                        if (hashSet.Add(item))
                            count++;
#else
                    for (var i = 0; i < array.Length; i++)
                        if (hashSet.Add(array[i]))
                            count++;
#endif
                    return count;
                }

                if (collection is List<TKey> list)
                {
#if NET5_0_OR_GREATER || NETCOREAPP3_0_OR_GREATER
                    var span = CollectionsMarshal.AsSpan(list);
                    foreach (ref readonly var item in span)
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

        public static int AddRange<TSource, TKey>(this HashSet<TKey> hashSet, IEnumerable<TSource> values, Func<TSource, TKey> keySelector)
        {
            if (hashSet == null) throw new ArgumentNullException(nameof(hashSet));
            if (values == null) throw new ArgumentNullException(nameof(values));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            var count = 0;

            if (values is ICollection<TSource> collection)
            {
                if (collection.Count == 0)
                    return 0;

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER || NET8_0_OR_GREATER || NET10_0_OR_GREATER
                hashSet.EnsureCapacity(hashSet.Count + collection.Count);
#endif

                if (collection is TSource[] array)
                {
#if NET5_0_OR_GREATER || NETCOREAPP3_0_OR_GREATER
                    var span = array.AsSpan();
                    foreach (ref readonly var item in span)
                        if (hashSet.Add(keySelector(item)))
                            count++;
#else
                    for (var i = 0; i < array.Length; i++)
                        if (hashSet.Add(keySelector(array[i])))
                            count++;
#endif
                    return count;
                }

                if (collection is List<TSource> list)
                {
#if NET5_0_OR_GREATER || NETCOREAPP3_0_OR_GREATER
                    var span = CollectionsMarshal.AsSpan(list);
                    foreach (ref readonly var item in span)
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

        public static int RemoveRange<TKey>(this HashSet<TKey> hashSet, IEnumerable<TKey> values)
        {
            if (hashSet == null) throw new ArgumentNullException(nameof(hashSet));
            if (values == null) throw new ArgumentNullException(nameof(values));

            var count = 0;

            if (values is ICollection<TKey> collection)
            {
                if (collection.Count == 0)
                    return 0;

                if (collection is TKey[] array)
                {
#if NET5_0_OR_GREATER || NETCOREAPP3_0_OR_GREATER
                    var span = array.AsSpan();
                    foreach (ref readonly var item in span)
                        if (hashSet.Remove(item))
                            count++;
#else
                    for (var i = 0; i < array.Length; i++)
                        if (hashSet.Remove(array[i]))
                            count++;
#endif
                    return count;
                }

                if (collection is List<TKey> list)
                {
#if NET5_0_OR_GREATER || NETCOREAPP3_0_OR_GREATER
                    var span = CollectionsMarshal.AsSpan(list);
                    foreach (ref readonly var item in span)
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

        public static int RemoveRange<TSource, TKey>(this HashSet<TKey> hashSet, IEnumerable<TSource> values, Func<TSource, TKey> keySelector)
        {
            if (hashSet == null) throw new ArgumentNullException(nameof(hashSet));
            if (values == null) throw new ArgumentNullException(nameof(values));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            var count = 0;

            if (values is ICollection<TSource> collection)
            {
                if (collection.Count == 0)
                    return 0;

                if (collection is TSource[] array)
                {
#if NET5_0_OR_GREATER || NETCOREAPP3_0_OR_GREATER
                    var span = array.AsSpan();
                    foreach (ref readonly var item in span)
                        if (hashSet.Remove(keySelector(item)))
                            count++;
#else
                    for (var i = 0; i < array.Length; i++)
                        if (hashSet.Remove(keySelector(array[i])))
                            count++;
#endif
                    return count;
                }

                if (collection is List<TSource> list)
                {
#if NET5_0_OR_GREATER || NETCOREAPP3_0_OR_GREATER
                    var span = CollectionsMarshal.AsSpan(list);
                    foreach (ref readonly var item in span)
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
    }
}
INNER_EOF

dotnet run -c Release --project src/Tedd.HashSetExtensions.Benchmarks/Tedd.HashSetExtensions.Benchmarks.csproj
