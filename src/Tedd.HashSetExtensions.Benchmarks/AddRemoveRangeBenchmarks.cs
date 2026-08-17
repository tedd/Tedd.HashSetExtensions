using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Collections.Generic;
using System.Linq;
using Tedd;
using Tedd.Archive;

[MemoryDiagnoser]
public class AddRemoveRangeBenchmarks
{
    private HashSet<int> _hashSetArchive = null!;
    private HashSet<int> _hashSetOptimized = null!;
    private List<int> _list = null!;
    private int[] _array = null!;

    [Params(100, 10000)]
    public int Size { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _list = Enumerable.Range(0, Size).ToList();
        _array = _list.ToArray();
    }

    [IterationSetup]
    public void IterationSetup()
    {
        _hashSetArchive = new HashSet<int>();
        _hashSetOptimized = new HashSet<int>();
    }

    [Benchmark(Baseline = true)]
    public int Archive_AddRangeList() => Tedd.Archive.AddRemoveRangeExtensions.AddRange(_hashSetArchive, _list);

    [Benchmark]
    public int Optimized_AddRangeList() => Tedd.AddRemoveRangeExtensions.AddRange(_hashSetOptimized, _list);

    [Benchmark]
    public int Archive_AddRangeArray() => Tedd.Archive.AddRemoveRangeExtensions.AddRange(_hashSetArchive, _array);

    [Benchmark]
    public int Optimized_AddRangeArray() => Tedd.AddRemoveRangeExtensions.AddRange(_hashSetOptimized, _array);
}
