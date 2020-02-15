using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic.CompilerServices;
using Tedd.RandomUtils;
using Xunit;

namespace Tedd.HashSetExtensions.Tests
{
    public class AddRemoveRangeTest
    {
        private const int ListSize = 1000;
        private const string Letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private FastRandom _rnd = new FastRandom();

        #region Helper methods
        private struct KV<TKey, TValue>
        {
            public TKey Key;
            public TValue Value;

            #region Overrides of ValueType

            /// <summary>Indicates whether this instance and a specified object are equal.</summary>
            /// <param name="obj">The object to compare with the current instance.</param>
            /// <returns>
            /// <see langword="true" /> if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, <see langword="false" />.</returns>
            public override bool Equals(object? obj)
            {
                var ob = obj as KV<TKey, TValue>?;
                if (ob == null)
                    return false;
                var o = ob.Value;
                return Key.Equals(o.Key) && Value.Equals(o.Value);
            }

            #endregion
        }

        private List<KV<string, int>> GenerateList()
        {
            var ret = new List<KV<string, int>>(ListSize);
            for (var i = 0; i < ListSize; i++)
            {
                var kv = new KV<string, int>()
                {
                    Key = _rnd.NextString(Letters, 10),
                    Value = i
                };
                ret.Add(kv);
            }

            return ret;
        }

        private void VerifyListsKey(HashSet<string> dic, List<string> list1, List<string> list2)
        {
            var l2c = 0;
            if (list2 != null)
                l2c = list2.Count;
            Assert.Equal(dic.Count, list1.Count + l2c);
            for (var i = 0; i < ListSize; i++)
            {
                Assert.Contains(list1[i], dic);
                if (list2 != null)
                    Assert.Contains(list2[i], dic);
            }
        }
        #endregion

        [Fact]
        public void SanityTests()
        {

            List<string> list = null;
            Assert.Throws<ArgumentException>(() => { list.ToHashSet(); });
            list = new List<string>();
            var h = list.ToHashSet();
            h.AddRange(list);
            h.RemoveRange(list);
            Assert.Throws<ArgumentException>(() => { h.AddRange(null); });
            Assert.Throws<ArgumentException>(() => { h.RemoveRange(null); });
            
        }

        #region No selector
        #region List
        [Fact]
        public void AddRangeList()
        {
            var list1 = GenerateList().Select(s => s.Key).ToList();
            var list2 = GenerateList().Select(s => s.Key).ToList();
            var h = new HashSet<string>();
            h.AddRange(list1);
            h.AddRange(list2);
            VerifyListsKey(h, list1, list2);
            h.RemoveRange(list1);
            VerifyListsKey(h, list2, null);
        }

        [Fact]
        public void AddRangeListComparer()
        {
            var list1 = GenerateList().Select(s => s.Key.ToLowerInvariant()).ToList();
            var list2 = GenerateList().Select(s => s.Key.ToLowerInvariant()).ToList();
            var h = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            h.AddRange(list1);
            h.AddRange(list2);
            list1 = list1.Select(s => s.ToUpperInvariant()).ToList();
            list2 = list2.Select(s => s.ToUpperInvariant()).ToList();
            VerifyListsKey(h, list1, list2);
            h.RemoveRange(list1);
            VerifyListsKey(h, list2, null);
        }
        #endregion

        #region Array
        [Fact]
        public void AddRangeArray()
        {
            var list1 = GenerateList().Select(s => s.Key).ToArray();
            var list2 = GenerateList().Select(s => s.Key).ToArray();
            var h = new HashSet<string>();
            h.AddRange(list1);
            h.AddRange(list2);
            VerifyListsKey(h, list1.ToList(), list2.ToList());
            h.RemoveRange(list1);
            VerifyListsKey(h, list2.ToList(), null);
        }

        [Fact]
        public void AddRangeArrayComparer()
        {
            var list1 = GenerateList().Select(s => s.Key.ToLowerInvariant()).ToArray();
            var list2 = GenerateList().Select(s => s.Key.ToLowerInvariant()).ToArray();
            var h = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            h.AddRange(list1);
            h.AddRange(list2);
            list1 = list1.Select(s => s.ToUpperInvariant()).ToArray();
            list2 = list2.Select(s => s.ToUpperInvariant()).ToArray();
            VerifyListsKey(h, list1.ToList(), list2.ToList());
            h.RemoveRange(list1);
            VerifyListsKey(h, list2.ToList(), null);
        }
        #endregion

        #region IEnumerable (HashSet)
        [Fact]
        public void AddRangeIEnumerable()
        {

            var list1 = GenerateList().Select(s => s.Key);
            var list2 = GenerateList().Select(s => s.Key);
            var h = new HashSet<string>();
            h.AddRange(list1);
            h.AddRange(list2);
            VerifyListsKey(h, list1.ToList(), list2.ToList());
            h.RemoveRange(list1);
            VerifyListsKey(h, list2.ToList(), null);
        }

        [Fact]
        public void AddRangeIEnumerableComparer()
        {
            var list1 = GenerateList().Select(s => s.Key.ToLowerInvariant());
            var list2 = GenerateList().Select(s => s.Key.ToLowerInvariant());
            var h = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            h.AddRange(list1);
            h.AddRange(list2);
            list1 = list1.Select(s => s.ToUpperInvariant());
            list2 = list2.Select(s => s.ToUpperInvariant());
            VerifyListsKey(h, list1.ToList(), list2.ToList());
            h.RemoveRange(list1);
            VerifyListsKey(h, list2.ToList(), null);
        }
        #endregion
        #endregion

        #region Selector
        #region List
        [Fact]
        public void AddRangeListSelector()
        {
            var list1 = GenerateList().ToList();
            var list2 = GenerateList().ToList();
            var h = new HashSet<string>();
            h.AddRange(list1, s => s.Key);
            h.AddRange(list2, s => s.Key);
            VerifyListsKey(h, list1.Select(s => s.Key).ToList(), list2.Select(s => s.Key).ToList());
            h.RemoveRange(list1, s => s.Key);
            VerifyListsKey(h, list2.Select(s => s.Key).ToList(), null);
        }

        [Fact]
        public void AddRangeListComparerSelector()
        {
            var list1 = GenerateList().ToList();
            var list2 = GenerateList().ToList();
            var h = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            h.AddRange(list1, s => s.Key.ToLowerInvariant());
            h.AddRange(list2, s => s.Key.ToLowerInvariant());
            VerifyListsKey(h, list1.Select(s => s.Key.ToUpperInvariant()).ToList(), list2.Select(s => s.Key).ToList());
            h.RemoveRange(list1, s => s.Key);
            VerifyListsKey(h, list2.Select(s => s.Key).ToList(), null);
        }
        #endregion

        #region Array
        [Fact]
        public void AddRangeArraySelector()
        {
            var list1 = GenerateList().ToList();
            var list2 = GenerateList().ToList();
            var h = new HashSet<string>();
            h.AddRange(list1.ToArray(), s => s.Key);
            h.AddRange(list2.ToArray(), s => s.Key);
            VerifyListsKey(h, list1.Select(s => s.Key).ToList(), list2.Select(s => s.Key).ToList());
            h.RemoveRange(list1.ToArray(), s => s.Key);
            VerifyListsKey(h, list2.Select(s => s.Key).ToList(), null);
        }

        [Fact]
        public void AddRangeArrayComparerSelector()
        {
            var list1 = GenerateList().ToList();
            var list2 = GenerateList().ToList();
            var h = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            h.AddRange(list1.ToArray(), s => s.Key.ToLowerInvariant());
            h.AddRange(list2.ToArray(), s => s.Key.ToLowerInvariant());
            VerifyListsKey(h, list1.Select(s => s.Key.ToUpperInvariant()).ToList(), list2.Select(s => s.Key).ToList());
            h.RemoveRange(list1.ToArray(), s => s.Key);
            VerifyListsKey(h, list2.Select(s => s.Key).ToList(), null);
        }
        #endregion

        #region IEnumerable (HashSet)
        [Fact]
        public void AddRangeIEnumerableSelector()
        {
            var list1 = GenerateList().ToList();
            var list2 = GenerateList().ToList();
            var h = new HashSet<string>();
            h.AddRange(list1.Select(s => s), s => s.Key);
            h.AddRange(list2.Select(s => s), s => s.Key);
            VerifyListsKey(h, list1.Select(s => s.Key).ToList(), list2.Select(s => s.Key).ToList());
            h.RemoveRange(list1.Select(s => s), s => s.Key);
            VerifyListsKey(h, list2.Select(s => s.Key).ToList(), null);
        }

        [Fact]
        public void AddRangeIEnumerableComparerSelector()
        {
            var list1 = GenerateList().ToList();
            var list2 = GenerateList().ToList();
            var h = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            h.AddRange(list1.Select(s => s), s => s.Key.ToLowerInvariant());
            h.AddRange(list2.Select(s => s), s => s.Key.ToLowerInvariant());
            VerifyListsKey(h, list1.Select(s => s.Key.ToUpperInvariant()).ToList(), list2.Select(s => s.Key).ToList());
            h.RemoveRange(list1.Select(s => s), s => s.Key);
            VerifyListsKey(h, list2.Select(s => s.Key).ToList(), null);
        }
        #endregion
        #endregion
    }
}