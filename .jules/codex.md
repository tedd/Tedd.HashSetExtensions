## 2024-06-18 - Documentation Drift: ToHashSet() Ambiguity in Modern .NET

**Observation:** Current `README.md` code examples specify using `.ToHashSet()` without a selector on `IEnumerable<T>`. In modern .NET contexts (such as .NET 9.0/10.0+), executing this method on generic enumerables via extension method syntax results in a compiler ambiguity error (CS0121) due to structural conflict with `System.Linq.Enumerable.ToHashSet<TSource>(IEnumerable<TSource>)`. The current examples therefore exhibit syntactic invalidity in modern environments. Furthermore, existing code snippets utilize outdated instantiation and initialization syntaxes, introducing pedagogical friction.

**Strategic Action:**
1. Revise instructional examples within `README.md` to utilize the selector syntax (e.g., `.ToHashSet(s => s)`) or explicitly address the disambiguation protocol when no selector is utilized.
2. Upgrade structural syntax across all documentation artifacts to utilize contemporary C# collection expressions (e.g., `[]`) and target-typed `new()`.
3. Introduce an explicit "Architectural Execution Flow" section to delineate how the extension structurally prioritizes array/list capacity allocation via index-based iteration.

## 2024-09-18 - Documentation Drift: Architectural Execution Flow Articulation

**Observation:** The README.md file exhibited epistemological deficits regarding the recent introduction of the `EnsureCapacity` and capacity constructor optimizations for `HashSet<T>` instances utilizing arrays and lists. This lack of architectural clarity required developers to independently deduce the O(1) allocation mechanism introduced for modern .NET targets.

**Strategic Action:** Explicitly articulated the "Capacity Pre-allocation" paradigm within the "Architectural Execution Flow" section of the README.md. This strictly differentiates the established paradigms (like deterministic indexing) from the recently implemented performance optimizations, effectively reducing pedagogical friction for developers utilizing .NET Standard 2.1 and .NET Core 3.0+.
