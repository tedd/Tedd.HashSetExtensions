using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace Tedd.HashSetExtensions.Benchmarks
{
    [MemoryDiagnoser]
    public class ContainsBenchmarks
    {
        private List<int>? _list;
        private int[]? _array;
        private HashSet<int>? _hashSet;

        [Params(10, 1000, 100000)]
        public int Count { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            _list = Enumerable.Range(0, Count).ToList();
            // In ContainsRange we break early if found, so we make sure it traverses fully to stress it
            _hashSet = new HashSet<int> { Count + 1 };
            _array = _list.ToArray();
        }

        [Benchmark(Baseline = true)]
        public void Archive_ContainsRange_List()
        {
            Tedd.Archive.ContainsRangeExtensions.ContainsRange(_hashSet, _list);
        }

        [Benchmark]
        public void Optimized_ContainsRange_List()
        {
            Tedd.ContainsRangeExtensions.ContainsRange(_hashSet, _list);
        }

        [Benchmark]
        public void Archive_ContainsRange_Array()
        {
            Tedd.Archive.ContainsRangeExtensions.ContainsRange(_hashSet, _array);
        }

        [Benchmark]
        public void Optimized_ContainsRange_Array()
        {
            Tedd.ContainsRangeExtensions.ContainsRange(_hashSet, _array);
        }
    }
}
