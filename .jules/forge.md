## 2024-10-02 - Package Metadata and Dependency Drift

**Observation:** The main project `Tedd.HashSetExtensions.csproj` lacks a `<PackageReadmeFile>` property and doesn't pack `README.md`. Also, the `<PackagePath>` for `LICENSE` is empty instead of being `\`. Test and Benchmark projects have outdated dependencies (e.g. `xunit`, `Microsoft.NET.Test.Sdk`, `BenchmarkDotNet`). `xunit` 2.4.0 is marked as legacy/deprecated.

**Strategic Action:** Update `Tedd.HashSetExtensions.csproj` to properly pack `README.md` and set `<PackagePath>\</PackagePath>` for both `LICENSE` and `README.md`. Modernize test and benchmark dependencies to their latest compatible stable versions to ensure robust testing and compatibility.
