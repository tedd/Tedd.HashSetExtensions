using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace Tedd.HashSetExtensions.Benchmarks
{
    [MemoryDiagnoser]
    public class ToHashSetBenchmarks
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
        public void Archive_ToHashSet_List()
        {
            Tedd.Archive.ToHashSetExtensions.ToHashSet(_list);
        }

        [Benchmark]
        public void Optimized_ToHashSet_List()
        {
            Tedd.ToHashSetExtensions.ToHashSet(_list);
        }

        [Benchmark]
        public void Archive_ToHashSet_Array()
        {
            Tedd.Archive.ToHashSetExtensions.ToHashSet(_array);
        }

        [Benchmark]
        public void Optimized_ToHashSet_Array()
        {
            Tedd.ToHashSetExtensions.ToHashSet(_array);
        }
    }
}
