## 2024-05-24 - Dependency and Framework Modernization

**Observation:** The test and benchmark projects have outdated dependencies (e.g. `Microsoft.NET.Test.Sdk`, `xunit`, `BenchmarkDotNet`) and only target `net8.0` even though pipeline covers `net10.0`. The main package (`Tedd.HashSetExtensions`) lacks `<GenerateDocumentationFile>` and explicit README packing via `<PackageReadmeFile>` metadata despite having XML documentation needs for its public API.
The azure pipeline was failing because when setting `workingDirectory: src`, the `projects` input to the `DotNetCoreCLI@2` task was not able to evaluate paths like `**/*Tests.csproj` accurately, causing the `No files matched the search pattern` error.

**Strategic Action:** Update `Tedd.HashSetExtensions.Tests.csproj` and `Tedd.HashSetExtensions.Benchmarks.csproj` to support `net8.0;net10.0` and bring all their NuGet package references to the latest stable versions. Add package metadata to `Tedd.HashSetExtensions.csproj` to include XML documentation generation and explicit `README.md` packaging. Remove the redundant `projects` input from the Azure Pipeline `DotNetCoreCLI@2` tasks where `workingDirectory: src` is already set.
