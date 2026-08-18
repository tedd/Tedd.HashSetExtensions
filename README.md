# Tedd.HashSetExtensions
.Net extension methods for ToHashSet&lt;T>().<br />
All methods implemented with and without both selector and comparer. Special handling of Array and List to avoid enumeration overhead. In line with how ToDictionary is implemented in .Net source code.

## Architectural Execution Flow

The core epistemological mechanism behind these extensions is type-checking against `ICollection<T>` prior to enumeration. When invoked, the execution flow operates as follows:

1. **Type Resolution:** The framework determines if the provided `IEnumerable<T>` implements `ICollection<T>`.
2. **Memory Pre-allocation (Modern .NET):** If an `ICollection<T>` is identified on modern frameworks (.NET Standard 2.1+, .NET Core 3.0+, .NET 5.0+), the framework proactively invokes `EnsureCapacity()` prior to population, mitigating sequential memory reallocation overhead.
3. **Deterministic Indexing (Established Paradigm):** If the structure resolves as a `T[]` (array) or `List<T>`, the framework bypasses standard `foreach` allocation overhead and utilizes a `for` loop. This leverages deterministic indexers (`array[i]` or `list[i]`), ensuring highly optimized, allocation-free iteration.
4. **Fallback Enumeration:** If the collection is a generic `IEnumerable<T>`, it gracefully defaults to standard enumeration.

*(Hypothetical enhancements regarding `ReadOnlySpan<T>` or vectorized SIMD execution are not currently implemented and remain under evaluation.)*

# Examples

All methods support a selector paradigm. A selector dictates the extraction or structural transformation of the entity prior to its integration into the hash set.

> **⚠️ Structural Disambiguation (.NET 9.0/10.0+):**
> Modern .NET SDKs inherently contain `System.Linq.Enumerable.ToHashSet<T>`. When invoking parameterless `.ToHashSet()` extensions on local variables containing `System.Linq`, the compiler encounters an ambiguous resolution (CS0121). To mitigate this structural anomaly, employ an identity selector (`s => s`) or explicitly reference the class paradigm (`Tedd.ToHashSetExtensions.ToHashSet(list)`).

## Selector example

```cs
Dictionary<string, int> dic = new()
{
    ["A"] = 1,
    ["B"] = 2
};

// A dictionary consists of KeyValuePair structures; we extract the Key component for the HashSet
HashSet<string> hashSet = dic.ToHashSet(s => s.Key);

dic["CC"] = 2;
// Or implement localized functional transformation logic
HashSet<string> hashSet2 = dic.ToHashSet(s => {
    if (s.Key == "CC")
        return "C";
    return s.Key;
});
```

## Methods

### ienumerable.ToHashSet(selector)
```cs
List<string> list = ["A", "B"];
// HashSet<string> hashSet = list.ToHashSet(); // Ambiguous resolution in modern .NET
HashSet<string> hashSet = list.ToHashSet(s => s); // Functional selector ensures deterministic resolution
bool thisIsTrue = hashSet.Contains("A");
bool thisIsFalse = hashSet.Contains("C");

// Append B again
list.Add("B");
HashSet<string> hashSet2 = list.ToHashSet(s => s);
// HashSet retains 2 entities; structural duplicates are discarded
int thisIsTwo = hashSet2.Count;
```

### ienumerable.ToHashSet(selector, comparer)
```cs
List<string> list = ["a", "b"];
HashSet<string> hashSet = list.ToHashSet(s => s, StringComparer.InvariantCultureIgnoreCase);
// HashSet structure: a, b

bool thisIsTrue = hashSet.Contains("a");
bool thisIsAlsoTrue = hashSet.Contains("A");
```

### hashset.ContainsRange(ienumerable)
```cs
List<string> list = ["A", "B", "C"];
HashSet<string> hashSet = list.ToHashSet(s => s);
// HashSet structure: A, B, C

List<string> otherList = ["A", "B"];

bool thisIsTrue = hashSet.ContainsRange(otherList);
```

### hashset.AddRange(ienumerable)
```cs
List<string> list1 = ["A", "B"];
HashSet<string> hashSet = list1.ToHashSet(s => s);
// HashSet structure: A, B

List<string> list2 = ["C", "D"];
hashSet.AddRange(list2);
// HashSet structure: A, B, C, D

bool thisIsTrue = hashSet.Contains("D");
```

### hashset.RemoveRange(ienumerable)
```cs
List<string> list1 = ["A", "B", "C", "D"];
HashSet<string> hashSet = list1.ToHashSet(s => s);
// HashSet structure: A, B, C, D

List<string> list2 = ["A", "B"];
hashSet.RemoveRange(list2);
// HashSet structure: C, D

bool thisIsFalse = hashSet.Contains("A");
bool thisIsAlsoFalse = hashSet.Contains("B");
```
