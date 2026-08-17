using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Collections.Generic;
using System.Linq;
using Tedd;
using Tedd.Archive;

[MemoryDiagnoser]
public class ToHashSetBenchmarks
{
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

    [Benchmark(Baseline = true)]
    public HashSet<int> Archive_ListToHashSet() => Tedd.Archive.ToHashSetExtensions.ToHashSet(_list);

    [Benchmark]
    public HashSet<int> Optimized_ListToHashSet() => Tedd.ToHashSetExtensions.ToHashSet(_list);

    [Benchmark]
    public HashSet<int> Archive_ArrayToHashSet() => Tedd.Archive.ToHashSetExtensions.ToHashSet(_array);

    [Benchmark]
    public HashSet<int> Optimized_ArrayToHashSet() => Tedd.ToHashSetExtensions.ToHashSet(_array);
}

public class Program
{
    public static void Main(string[] args)
    {
        BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
    }
}
