using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Collections.Generic;
using System.Linq;

[MemoryDiagnoser]
public class FinalBenchmarks
{
    private int[] _array = null!;
    private List<int> _list = null!;

    [Params(1000)]
    public int Size { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _array = Enumerable.Range(0, Size).ToArray();
        _list = _array.ToList();
    }

    [Benchmark(Baseline = true)]
    public HashSet<int> LegacyToHashSet()
    {
        return Tedd.Archive.ToHashSetExtensions.ToHashSet(_array);
    }

    [Benchmark]
    public HashSet<int> OptimizedToHashSet()
    {
        return Tedd.ToHashSetExtensions.ToHashSet(_array);
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<FinalBenchmarks>();
    }
}
