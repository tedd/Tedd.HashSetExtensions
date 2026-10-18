## 2024-06-18 - Documentation Drift: ToHashSet() Ambiguity in Modern .NET

**Observation:** Current `README.md` code examples specify using `.ToHashSet()` without a selector on `IEnumerable<T>`. In modern .NET contexts (such as .NET 9.0/10.0+), executing this method on generic enumerables via extension method syntax results in a compiler ambiguity error (CS0121) due to structural conflict with `System.Linq.Enumerable.ToHashSet<TSource>(IEnumerable<TSource>)`. The current examples therefore exhibit syntactic invalidity in modern environments. Furthermore, existing code snippets utilize outdated instantiation and initialization syntaxes, introducing pedagogical friction.

**Strategic Action:**
1. Revise instructional examples within `README.md` to utilize the selector syntax (e.g., `.ToHashSet(s => s)`) or explicitly address the disambiguation protocol when no selector is utilized.
2. Upgrade structural syntax across all documentation artifacts to utilize contemporary C# collection expressions (e.g., `[]`) and target-typed `new()`.
3. Introduce an explicit "Architectural Execution Flow" section to delineate how the extension structurally prioritizes array/list capacity allocation via index-based iteration.

## 2026-10-18 - Pre-allocation and Functional Selector Articulation
**Observation:** The README.md failed to articulate the framework's optimal pre-allocation capabilities using `ICollection<T>.Count` on modern .NET targets, resulting in a conceptual drift. Furthermore, the operational capability of functional selector overloads for `ContainsRange`, `AddRange`, and `RemoveRange`, as well as their return types (capturing the number of elements processed), was structurally undocumented, leading to epistemological friction for developers.
**Strategic Action:** Synchronized the "Architectural Execution Flow" section to explicitly detail the "Pre-Allocation Optimization" paradigm. Symmetrically updated all example modules to utilize selector overloads and capture functional return types, ensuring absolute epistemological parity and mitigating future ambiguity regarding modern .NET memory utilization and API features.

## 2026-10-18 - CI Configuration Path Resolution Fix
**Observation:** The Azure Pipelines (`azure-pipelines.yml`) configuration was failing with 'No files matched the search pattern'. This occurred because the `DotNetCoreCLI@2` tasks were using a glob pattern (`*.sln`) in conjunction with `workingDirectory: src`. According to known pipeline behavior, the `projects` input glob pattern resolves relative to the repository root and ignores `workingDirectory`.
**Strategic Action:** Modified `azure-pipelines.yml` to specify full relative paths in the `projects` input (e.g., `src/*.sln` and `src/**/*Tests.csproj`) and removed the redundant `workingDirectory` argument, resolving the pipeline file resolution issue.
