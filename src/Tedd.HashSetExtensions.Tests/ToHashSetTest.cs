using System;
using System.Collections.Generic;
using System.Linq;
using Tedd.RandomUtils;
using Xunit;

namespace Tedd.HashSetExtensions.Tests
{
    public class ToHashSetTest
    {
        private const int ListSize = 1000;
        private const string Letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private FastRandom _rnd = new FastRandom();

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

        private void SetUpLists(out List<KV<string, int>> singleList, out List<KV<string, int>> dupList)
        {
            singleList = new List<KV<string, int>>(ListSize);
            dupList = new List<KV<string, int>>(ListSize * 2);
            for (var i = 0; i < ListSize; i++)
            {
                var kv = new KV<string, int>()
                {
                    Key = _rnd.NextString(Letters, 10),
                    Value = i
                };
                singleList.Add(kv);
                dupList.Add(kv);
                //if (i % 3 == 0)
                //    dupList.Add(kv);
            }
        }

        private void VerifyListsKey(List<KV<string, int>> singleList, HashSet<string> dic)
        {
            Assert.Equal(dic.Count, singleList.Count);
            for (var i = 0; i < ListSize; i++)
            {
                var kv = singleList[i];
                Assert.True(dic.TryGetValue(kv.Key, out var val));
            }
        }
        private void VerifyListsKeyValue(List<KV<string, int>> singleList, HashSet<string> dic)
        {
            Assert.Equal(dic.Count, singleList.Count);
            for (var i = 0; i < ListSize; i++)
            {
                var kv = singleList[i];
                Assert.True(dic.TryGetValue(kv.Key, out var val));
            }
        }

        #region Selector
        #region List
        [Fact]
        public void ListToHashSetKey()
        {
            SetUpLists(out var singleList, out var dupList);
            var dic = dupList.ToHashSet(k => k.Key);
            VerifyListsKey(singleList, dic);
        }

        [Fact]
        public void ListToHashSetKeyComparer()
        {
            SetUpLists(out var singleList, out var dupList);
            var dic = dupList.ToHashSet(k => k.Key.ToLowerInvariant(), StringComparer.OrdinalIgnoreCase);
            VerifyListsKey(singleList, dic);
        }


        #endregion
        #region Array
        [Fact]
        public void ArrayToHashSetKey()
        {
            SetUpLists(out var singleList, out var dupList);
            var dic = dupList.ToArray().ToHashSet(k => k.Key);
            VerifyListsKey(singleList, dic);
        }


        [Fact]
        public void ArrayToHashSetKeyComparer()
        {
            SetUpLists(out var singleList, out var dupList);
            var dic = dupList.ToArray().ToHashSet(k => k.Key.ToLowerInvariant(), StringComparer.OrdinalIgnoreCase);
            VerifyListsKey(singleList, dic);
        }


        #endregion
        #region IEnumerable (HashSet)
        [Fact]
        public void IEnumerableToHashSetKey()
        {
            SetUpLists(out var singleList, out var dupList);
            var dic = dupList.ToHashSet().ToHashSet(k => k.Key);
            VerifyListsKey(singleList, dic);
        }


        [Fact]
        public void IEnumerableToHashSetKeyComparer()
        {
            SetUpLists(out var singleList, out var dupList);
            var dic = dupList.ToHashSet().ToHashSet(k => k.Key.ToLowerInvariant(), StringComparer.OrdinalIgnoreCase);
            VerifyListsKey(singleList, dic);
        }


        #endregion
        #endregion
        #region No selector
        #region List
        [Fact]
        public void ListToHashSet()
        {
            SetUpLists(out var singleList, out var dupList);
            var dic = (dupList.Select(s=>s.Key).ToList()).ToHashSet();
            VerifyListsKey(singleList, dic);
        }

        [Fact]
        public void ListToHashSetComparer()
        {
            SetUpLists(out var singleList, out var dupList);
            var dic = (dupList.Select(s => s.Key.ToLowerInvariant()).ToList()).ToHashSet(StringComparer.OrdinalIgnoreCase);
            VerifyListsKey(singleList, dic);
        }


        #endregion
        #region Array
        [Fact]
        public void ArrayToHashSet()
        {
            SetUpLists(out var singleList, out var dupList);
            var dic = (dupList.Select(s => s.Key).ToArray()).ToHashSet();
            VerifyListsKey(singleList, dic);
        }


        [Fact]
        public void ArrayToHashSetComparer()
        {
            SetUpLists(out var singleList, out var dupList);
            var dic = (dupList.Select(s => s.Key.ToLowerInvariant()).ToArray()).ToHashSet(StringComparer.OrdinalIgnoreCase);
            VerifyListsKey(singleList, dic);
        }


        #endregion
        #region IEnumerable (HashSet)
        [Fact]
        public void IEnumerableToHashSet()
        {
            SetUpLists(out var singleList, out var dupList);
            var dic = (dupList.Select(s => s.Key)).ToHashSet();
            VerifyListsKey(singleList, dic);
        }


        [Fact]
        public void IEnumerableToHashSetComparer()
        {
            SetUpLists(out var singleList, out var dupList);
            var dic = (dupList.Select(s => s.Key.ToLowerInvariant())).ToHashSet(StringComparer.OrdinalIgnoreCase);
            VerifyListsKey(singleList, dic);
        }


        #endregion
        #endregion
    }
}