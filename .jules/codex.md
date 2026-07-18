## 2024-06-18 - Documentation Drift: ToHashSet() Ambiguity in Modern .NET

**Observation:** Current `README.md` code examples specify using `.ToHashSet()` without a selector on `IEnumerable<T>`. In modern .NET contexts (such as .NET 9.0/10.0+), executing this method on generic enumerables via extension method syntax results in a compiler ambiguity error (CS0121) due to structural conflict with `System.Linq.Enumerable.ToHashSet<TSource>(IEnumerable<TSource>)`. The current examples therefore exhibit syntactic invalidity in modern environments. Furthermore, existing code snippets utilize outdated instantiation and initialization syntaxes, introducing pedagogical friction.

**Strategic Action:**
1. Revise instructional examples within `README.md` to utilize the selector syntax (e.g., `.ToHashSet(s => s)`) or explicitly address the disambiguation protocol when no selector is utilized.
2. Upgrade structural syntax across all documentation artifacts to utilize contemporary C# collection expressions (e.g., `[]`) and target-typed `new()`.
3. Introduce an explicit "Architectural Execution Flow" section to delineate how the extension structurally prioritizes array/list capacity allocation via index-based iteration.

## 2024-06-25 - Architectural Execution Flow Drift

**Observation:** The `README.md` documentation incorrectly states that the framework bypasses standard `foreach` allocation overhead by utilizing a `for` loop with deterministic indexers (`array[i]` or `list[i]`). The documentation also lists `ReadOnlySpan<T>` optimizations as a hypothetical enhancement. However, the current framework implementation (since the `Bolt` optimizations) leverages `.AsSpan()` and `CollectionsMarshal.AsSpan(list)` for direct memory access via raw pointer arithmetic in modern .NET environments, making the `ReadOnlySpan<T>` reference factually inaccurate and obsolete. Furthermore, `EnsureCapacity` is actively utilized to eliminate `O(log N)` rehashing.

**Strategic Action:** Revised the `Architectural Execution Flow` section in `README.md` to accurately articulate the pre-allocation (`EnsureCapacity`) and deterministic memory access paradigms using `Span<T>` on modern .NET frameworks, explicitly distinguishing this from legacy fallback mechanisms.
