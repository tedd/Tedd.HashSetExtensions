using System;
using System.Collections.Generic;
using Tedd;

class Program
{
    static void Main()
    {
        // Example 1
        Dictionary<string, int> dictionary = new()
        {
            ["A"] = 1,
            ["B"] = 2
        };

        HashSet<string> hashSet1 = dictionary.ToHashSet(kvp => kvp.Key);

        dictionary.Add("CC", 2);

        HashSet<string> complexHashSet = dictionary.ToHashSet(kvp =>
        {
            if (kvp.Key == "CC")
                return "C";
            return kvp.Key;
        });

        // Example 2
        List<string> sourceList1 = ["A", "B"];
        HashSet<string> hashSet2 = sourceList1.ToHashSet();

        bool containsA = hashSet2.Contains("A");
        bool containsC = hashSet2.Contains("C");

        sourceList1.Add("B");
        HashSet<string> projectedHashSet = sourceList1.ToHashSet(s => s);
        int elementCount = projectedHashSet.Count;

        // Example 3
        List<string> sourceList2 = ["a", "b"];
        HashSet<string> hashSet3 = sourceList2.ToHashSet(s => s, StringComparer.InvariantCultureIgnoreCase);

        bool containsLower = hashSet3.Contains("a");
        bool containsUpper = hashSet3.Contains("A");

        // Example 4
        HashSet<string> hashSet4 = ["A", "B", "C"];
        List<string> searchList = ["A", "B"];
        bool containsAny = hashSet4.ContainsRange(searchList);

        // Example 5
        HashSet<string> hashSet5 = ["A", "B"];
        List<string> additionalElements = ["C", "D"];
        int addedCount = hashSet5.AddRange(additionalElements);
        bool containsD = hashSet5.Contains("D");

        // Example 6
        HashSet<string> hashSet6 = ["A", "B", "C", "D"];
        List<string> removalElements = ["A", "B"];
        int removedCount = hashSet6.RemoveRange(removalElements);
        bool containsA2 = hashSet6.Contains("A");
    }
}
