# Tedd.HashSetExtensions
.Net extension methods for ToHashSet&lt;T>()

## Examples
### ToHashSet(selector)
```cs
var list = new List<string>();
list.Add("A");
list.Add("B");
var hashSet = list.ToHashSet(s => s);
var thisIsTrue = hashSet.Contains("A");
var thisIsFalse = hashSet.Contains("C");

// Add B again
list.Add("B");
var hashSet2 = list.ToHashSet(s => s);
// HashSet only cointains 2 items because duplicates are ignored
var thisIsTwo = hashSet2.Count;

```
### ToHashSet(selector, comparer)
```cs
var list = new List<string>();
list.Add("a");
list.Add("b");
var hashSet = list.ToHashSet(s => s, StringComparer.InvariantCultureIgnoreCase);
var thisIsTrue = hashSet.Contains("a");
var thisIsAlsoTrue = hashSet.Contains("A");
```