using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace Tedd.HashSetExtensions.Benchmarks
{
    [MemoryDiagnoser]
    public class AddRemoveRangeBenchmarks
    {
        private List<int>? _list;
        private int[]? _array;

        [Params(10, 1000, 100000)]
        public int Count { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            _list = Enumerable.Range(0, Count).ToList();
            _array = _list.ToArray();
        }

        [Benchmark(Baseline = true)]
        public void Archive_AddRange_List()
        {
            var hashSet = new HashSet<int>();
            Tedd.Archive.AddRemoveRangeExtensions.AddRange(hashSet, _list);
        }

        [Benchmark]
        public void Optimized_AddRange_List()
        {
            var hashSet = new HashSet<int>();
            Tedd.AddRemoveRangeExtensions.AddRange(hashSet, _list);
        }

        [Benchmark]
        public void Archive_AddRange_Array()
        {
            var hashSet = new HashSet<int>();
            Tedd.Archive.AddRemoveRangeExtensions.AddRange(hashSet, _array);
        }

        [Benchmark]
        public void Optimized_AddRange_Array()
        {
            var hashSet = new HashSet<int>();
            Tedd.AddRemoveRangeExtensions.AddRange(hashSet, _array);
        }
    }
}
