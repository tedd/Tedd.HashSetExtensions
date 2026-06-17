## 2024-06-17 - HashSet Extensions Allocation & Time Overhead

**Observation:** The previous implementation of `AddRange`, `RemoveRange`, `ToHashSet`, and `ContainsRange` utilized `foreach` on `IEnumerable<T>` and indexers on `IList<T>`. When iterating `List<T>`, using indexers incurs bounds-checking per element, and for generic collections, virtual method dispatch. When adding to a `HashSet<T>` sequentially, `HashSet<T>` incurs intermediate allocations and resizing overhead if the capacity is not pre-allocated.

**Strategic Action:**
- Substituted `for` loop on lists with `CollectionsMarshal.AsSpan(list)` to bypass bounds-checking and provide direct memory access, optimizing iteration from `O(N)` bounds-checked to raw pointer arithmetic.
- Substituted `for` loop on arrays with `.AsSpan()` for equivalent performance.
- Pre-allocated `HashSet<T>` internal arrays leveraging `EnsureCapacity()` for collections of known sizes, eliminating O(log N) rehashing events during `AddRange`.
- In `ContainsRange`, optimized similarly to bypass iteration overheads.
