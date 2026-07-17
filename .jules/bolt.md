## 2024-06-17 - HashSet Extensions Allocation & Time Overhead

**Observation:** The previous implementation of `AddRange`, `RemoveRange`, `ToHashSet`, and `ContainsRange` utilized `foreach` on `IEnumerable<T>` and indexers on `IList<T>`. When iterating `List<T>`, using indexers incurs bounds-checking per element, and for generic collections, virtual method dispatch. When adding to a `HashSet<T>` sequentially, `HashSet<T>` incurs intermediate allocations and resizing overhead if the capacity is not pre-allocated.

**Strategic Action:**
- Substituted `for` loop on lists with `CollectionsMarshal.AsSpan(list)` to bypass bounds-checking and provide direct memory access, optimizing iteration from `O(N)` bounds-checked to raw pointer arithmetic.
- Substituted `for` loop on arrays with `.AsSpan()` for equivalent performance.
- Pre-allocated `HashSet<T>` internal arrays leveraging `EnsureCapacity()` for collections of known sizes, eliminating O(log N) rehashing events during `AddRange`.
- In `ContainsRange`, optimized similarly to bypass iteration overheads.
## 2026-07-17 - ToHashSet Capacity Pre-allocation
**Observation:** The `ToHashSet` extensions enumerated arrays and lists manually into a default `HashSet<T>` without supplying the known capacity. This resulted in hidden internal array resizing and increased GC pressure, particularly for larger collections (e.g., benchmark showed ~0.56x CPU execution time and ~0.30x allocated memory when capacity was specified).
**Strategic Action:** Conditionally utilized the `HashSet<T>(int capacity, IEqualityComparer<T>? comparer)` constructor when target frameworks support it (`NETSTANDARD2_1_OR_GREATER` / `NETCOREAPP`). Implemented via preprocessor directives to maintain backward compatibility for older frameworks like `net462` and `netstandard2.0`.
