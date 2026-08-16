using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xunit;

namespace Tedd.HashSetExtensions.Tests
{
    public class AegisTest
    {
        public static IEnumerable<object[]> GetNullCollectionData()
        {
            yield return new object[] { null };
        }

        [Theory]
        [MemberData(nameof(GetNullCollectionData))]
        public void AddRemoveRange_ArgumentNullExceptions(IEnumerable<int> nullValues)
        {
            HashSet<int> hashSet = null;
            IEnumerable<int> values = new[] { 1, 2, 3 };
            Func<int, int> selector = i => i;

            Assert.Throws<ArgumentException>(() => new HashSet<int>().AddRange(nullValues));
            Assert.Throws<ArgumentException>(() => new HashSet<int>().AddRange(nullValues, selector));
            Assert.Throws<ArgumentException>(() => new HashSet<int>().RemoveRange(nullValues));
            Assert.Throws<ArgumentException>(() => new HashSet<int>().RemoveRange(nullValues, selector));

            Assert.Throws<NullReferenceException>(() => hashSet.AddRange(values));
            Assert.Throws<NullReferenceException>(() => hashSet.AddRange(values, selector));
            Assert.Throws<NullReferenceException>(() => hashSet.RemoveRange(values));
            Assert.Throws<NullReferenceException>(() => hashSet.RemoveRange(values, selector));
        }

        [Theory]
        [MemberData(nameof(GetNullCollectionData))]
        public void ContainsRange_ArgumentNullExceptions(IEnumerable<int> nullValues)
        {
            HashSet<int> hashSet = null;
            IEnumerable<int> values = new[] { 1, 2, 3 };
            Func<int, int> selector = i => i;

            Assert.Throws<ArgumentException>(() => new HashSet<int>().ContainsRange(nullValues));
            Assert.Throws<ArgumentException>(() => new HashSet<int>().ContainsRange(nullValues, selector));

            Assert.Throws<NullReferenceException>(() => hashSet.ContainsRange(values));
            Assert.Throws<NullReferenceException>(() => hashSet.ContainsRange(values, selector));
        }

        [Theory]
        [MemberData(nameof(GetNullCollectionData))]
        public void ToHashSet_ArgumentExceptions(IEnumerable<int> nullValues)
        {
            IEnumerable<int> values = new[] { 1, 2, 3 };
            Func<int, int> nullSelector = null;
            Func<int, int> selector = i => i;

            Assert.Throws<ArgumentException>(() => nullValues.ToHashSet());
            Assert.Throws<ArgumentException>(() => nullValues.ToHashSet(selector));
            Assert.Throws<ArgumentException>(() => values.ToHashSet(nullSelector));
        }

        public static IEnumerable<object[]> GetEmptyCollectionData()
        {
            yield return new object[] { new Collection<int>() };
        }

        [Theory]
        [MemberData(nameof(GetEmptyCollectionData))]
        public void AddRemoveRange_EmptyCollection(ICollection<int> emptyCollection)
        {
            var hashSet = new HashSet<int>();

            Assert.Equal(0, hashSet.AddRange(emptyCollection));
            Assert.Equal(0, hashSet.AddRange(emptyCollection, i => i));
            Assert.Equal(0, hashSet.RemoveRange(emptyCollection));
            Assert.Equal(0, hashSet.RemoveRange(emptyCollection, i => i));
        }

        [Theory]
        [MemberData(nameof(GetEmptyCollectionData))]
        public void ToHashSet_EmptyCollection(ICollection<int> emptyCollection)
        {
            Assert.Empty(emptyCollection.ToHashSet());
            Assert.Empty(emptyCollection.ToHashSet(i => i));
        }

        [Theory]
        [MemberData(nameof(GetEmptyCollectionData))]
        public void ContainsRange_EmptyCollection(ICollection<int> emptyCollection)
        {
            var hashSet = new HashSet<int>();
            Assert.False(hashSet.ContainsRange(emptyCollection));
            Assert.False(hashSet.ContainsRange(emptyCollection, i => i));

            var nonEmptyCollection = new Collection<int> { 1, 2, 3 };
            Assert.False(hashSet.ContainsRange(nonEmptyCollection));
            Assert.False(hashSet.ContainsRange(nonEmptyCollection, i => i));
        }

        [Theory]
        [InlineData(1, 2, 3)]
        public void AddRemoveRange_ICollection_NotArrayOrList(int a, int b, int c)
        {
            var hashSet = new HashSet<int>();
            var collection = new Collection<int> { a, b, c };

            Assert.Equal(3, hashSet.AddRange(collection));
            Assert.Equal(3, hashSet.RemoveRange(collection));

            Assert.Equal(3, hashSet.AddRange(collection, i => i));
            Assert.Equal(3, hashSet.RemoveRange(collection, i => i));
        }

        [Theory]
        [InlineData(2, 4)]
        public void ContainsRange_ICollection_NotArrayOrList(int a, int b)
        {
            var hashSet = new HashSet<int> { 1, 2, 3 };
            var collection = new Collection<int> { a, b }; // Contains 2
            var missingCollection = new Collection<int> { 4, 5 }; // Does not contain any

            Assert.True(hashSet.ContainsRange(collection));
            Assert.False(hashSet.ContainsRange(missingCollection));

            Assert.True(hashSet.ContainsRange(collection, i => i));
            Assert.False(hashSet.ContainsRange(missingCollection, i => i));
        }

        [Theory]
        [InlineData(2, 2, true)]
        [InlineData(4, 2, false)]
        public void ContainsRange_IEnumerable_NotCollection(int start, int count, bool expected)
        {
            var hashSet = new HashSet<int> { 1, 2, 3 };
            var enumerable = Enumerable.Range(start, count);

            Assert.Equal(expected, hashSet.ContainsRange(enumerable));
            Assert.Equal(expected, hashSet.ContainsRange(enumerable, i => i));
        }

        [Theory]
        [InlineData(4, 5)]
        public void ContainsRange_NoMatch_ReturnsFalse(int a, int b)
        {
            var hashSet = new HashSet<int> { 1, 2, 3 };

            var pool = ArrayPool<int>.Shared;
            var array = pool.Rent(2);
            try
            {
                array[0] = a;
                array[1] = b;
                var span = array.AsSpan(0, 2);
                var arr = span.ToArray();

                var list = new List<int> { a, b };

                Assert.False(hashSet.ContainsRange(arr));
                Assert.False(hashSet.ContainsRange(list));

                Assert.False(hashSet.ContainsRange(arr, i => i));
                Assert.False(hashSet.ContainsRange(list, i => i));
            }
            finally
            {
                pool.Return(array);
            }
        }

        [Fact]
        public void AddRemoveRange_ExistingElements_ReturnsCorrectCount()
        {
            var pool = ArrayPool<int>.Shared;
            var array = pool.Rent(3);
            try
            {
                array[0] = 1;
                array[1] = 2;
                array[2] = 3;
                var arr = array.AsSpan(0, 3).ToArray();

                var list = new List<int> { 1, 2, 3 };
                var enumerable = Enumerable.Range(1, 3);

                Assert.Equal(1, new HashSet<int> { 1, 2 }.AddRange(arr));
                Assert.Equal(1, new HashSet<int> { 1, 2 }.AddRange(list));
                Assert.Equal(1, new HashSet<int> { 1, 2 }.AddRange(enumerable));

                Assert.Equal(1, new HashSet<int> { 1, 2 }.AddRange(arr, i => i));
                Assert.Equal(1, new HashSet<int> { 1, 2 }.AddRange(list, i => i));
                Assert.Equal(1, new HashSet<int> { 1, 2 }.AddRange(enumerable, i => i));

                Assert.Equal(2, new HashSet<int> { 1, 2 }.RemoveRange(arr));
                Assert.Equal(2, new HashSet<int> { 1, 2 }.RemoveRange(list));
                Assert.Equal(2, new HashSet<int> { 1, 2 }.RemoveRange(enumerable));

                Assert.Equal(2, new HashSet<int> { 1, 2 }.RemoveRange(arr, i => i));
                Assert.Equal(2, new HashSet<int> { 1, 2 }.RemoveRange(list, i => i));
            }
            finally
            {
                pool.Return(array);
            }
        }

        [Fact]
        public void ToHashSet_WithSelector_NotICollection()
        {
            var source = Enumerable.Range(1, 3);
            var result = source.ToHashSet(i => i.ToString(), StringComparer.Ordinal);
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void ToHashSet_Collection_NotArrayOrList_WithSelector()
        {
            var collection = new Collection<int> { 1, 2, 3 };
            var result = collection.ToHashSet(i => i.ToString(), StringComparer.Ordinal);
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void ToHashSet_WithSelector_NotICollection_ReturnsHashSet()
        {
            var enumerator = Enumerable.Range(1, 3).Where(i => i > 0);
            var result = enumerator.ToHashSet(i => i.ToString(), StringComparer.Ordinal);
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void ToHashSet_NullComparer_Fallback()
        {
            var pool = ArrayPool<int>.Shared;
            var array = pool.Rent(3);
            try
            {
                array[0] = 1;
                array[1] = 2;
                array[2] = 3;
                var arr = array.AsSpan(0, 3).ToArray();

                var result1 = arr.ToHashSet(i => i, null);
                var result2 = new List<int> { 1, 2, 3 }.ToHashSet(i => i, null);
                Assert.Equal(3, result1.Count);
                Assert.Equal(3, result2.Count);
            }
            finally
            {
                pool.Return(array);
            }
        }
    }
}
