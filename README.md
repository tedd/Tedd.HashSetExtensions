# Tedd.HashSetExtensions
.Net extension methods for ToHashSet&lt;T>().<br />
All methods implemented with and without both selector and comparer. Special handling of Array and List to avoid enumeration overhead. In line with how ToDictionary is implemented in .Net source code.

# Examples

All methods supports selector. A selector is used to pick or transform the object before it is added to the hash.

## Selector example

```cs
var dic = new Dictionary<string, int>();
dic.Add("A", 1);
dic.Add("B", 2);

// A dictionary consists of KeyValuePair and we pick the Key portion of that for our HashSet
var hashSet = dic.ToHashSet(s => s.Key);

dic.Add("CC", 2);
// Or we can have more complex logic
var hashSet2 = dic.ToHashSet(s => {
	if (s.Key == "CC")
		return "C";
	return s.Key;
});

```

## Methods

### ienumerable.ToHashSet()
```cs
var list = new List<string>();
list.Add("A");
list.Add("B");
var hashSet = list.ToHashSet(); // No selector needed
var thisIsTrue = hashSet.Contains("A");
var thisIsFalse = hashSet.Contains("C");

// Add B again
list.Add("B");
var hashSet2 = list.ToHashSet(s => s); // Using selector
// HashSet only cointains 2 items because duplicates are ignored
var thisIsTwo = hashSet2.Count;
```

### ienumerable.ToHashSet(selector, comparer)
```cs
var list = new List<string>();
list.Add("a");
list.Add("b");
var hashSet = list.ToHashSet(s => s, StringComparer.InvariantCultureIgnoreCase);
// HashSet now contains: a, b

var thisIsTrue = hashSet.Contains("a");
var thisIsAlsoTrue = hashSet.Contains("A");
```

### hashset.ContainsRange(ienumerable)
```cs
var list = new List<string>();
list.Add("A");
list.Add("B");
list.Add("C");
var hashSet = list.ToHashSet(s => s);
// HashSet now contains: A, B, C

var otherList = new List<string>();
otherList.Add("A");
otherList.Add("B");

var thisIsTrue = hashSet.ContainsRange(otherList);
```

### hashset.AddRange(ienumerable)
```cs
var list1 = new List<string>();
list1.Add("A");
list1.Add("B");
var hashSet = list1.ToHashSet(s => s);
// HashSet now contains: A, B

var list2 = new List<string>();
list.Add("C");
list.Add("D");
hashSet.AddRange(list2);
// HashSet now contains: A, B, C, D

var thisIsTrue = hashSet.Contains("D");
```

### hashset.RemoveRange(ienumerable)
```cs
var list1 = new List<string>();
list1.Add("A");
list1.Add("B");
list1.Add("C");
list1.Add("D");
var hashSet = list1.ToHashSet();
// HashSet now contains: A, B, C, D

var list2 = new List<string>();
list.Add("A");
list.Add("B");
hashSet.RemoveRange(list2);
// HashSet now contains: B, C


var thisIsFalse = hashSet.Contains("A");
var thisIsAlsoFalse = hashSet.Contains("B");
```
