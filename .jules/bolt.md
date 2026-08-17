## 2024-08-17 - HashSet Pre-allocation Optimization

**Observation:** The `Tedd.HashSetExtensions` library handles arrays and `List<T>` elements using direct indexers to avoid enumeration overhead. However, when constructing new `HashSet<T>` instances, it does not utilize the capacity-based constructor (`public HashSet(int capacity, IEqualityComparer<T> comparer)`) introduced in .NET 4.7.2 / .NET Standard 2.1. This causes the internal buckets array of the `HashSet` to dynamically resize during insertion, resulting in unnecessary heap allocations (GC pressure) and copying operations (O(n)).

**Strategic Action:** Implement the capacity constructor for target frameworks `.NET Standard 2.1` and `.NET 8.0/10.0+` using preprocessor directives (`#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER`). The `ICollection<T>.Count` property allows for O(1) determination of the required capacity. For lists and arrays, this will allow the `HashSet` to pre-allocate its internal buffers precisely, achieving O(1) allocation overhead per set creation.

## 2024-08-17 - Added capacity constructors to HashSet

**Observation:** The previous optimization was verified to be correct and reduced allocations, but there were minor issues: GetHashCode() implementation in test models threw NotImplementedException crashing tests, and only one benchmark was wired up in the Program runner.

**Strategic Action:** Implemented proper GetHashCode implementation via `HashCode.Combine(Key, Value)` for test objects to prevent crashing HashSet initialization during tests. Updated the Benchmark Runner to use `BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args)` so all benchmark classes in the assembly can be run.
## 2024-08-17 - Azure pipelines Nuget fix

**Observation:** The Azure pipeline build was failing on restore because the CI runners did not have a nuget tool installer setup step, causing `dotnet restore` under `DotNetCoreCLI@2` task to fail due to missing nuget executables.

**Strategic Action:** Explicitly added the `NuGetToolInstaller@1` task before the build/restore commands in the `azure-pipelines.yml`.
