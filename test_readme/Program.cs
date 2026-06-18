using System;
using System.Collections.Generic;
using Tedd;

class Program
{
    static void Main()
    {
        // Selector example
        Dictionary<string, int> dic = new()
        {
            ["A"] = 1,
            ["B"] = 2
        };

        HashSet<string> hashSet1 = dic.ToHashSet(s => s.Key);

        dic["CC"] = 2;
        HashSet<string> hashSet2 = dic.ToHashSet(s => {
            if (s.Key == "CC")
                return "C";
            return s.Key;
        });

        // ienumerable.ToHashSet(selector)
        List<string> list1 = ["A", "B"];
        HashSet<string> hashSet3 = list1.ToHashSet(s => s);
        bool thisIsTrue1 = hashSet3.Contains("A");
        bool thisIsFalse1 = hashSet3.Contains("C");

        list1.Add("B");
        HashSet<string> hashSet4 = list1.ToHashSet(s => s);
        int thisIsTwo = hashSet4.Count;

        // ienumerable.ToHashSet(selector, comparer)
        List<string> list2 = ["a", "b"];
        HashSet<string> hashSet5 = list2.ToHashSet(s => s, StringComparer.InvariantCultureIgnoreCase);

        bool thisIsTrue2 = hashSet5.Contains("a");
        bool thisIsAlsoTrue = hashSet5.Contains("A");

        // hashset.ContainsRange(ienumerable)
        List<string> list3 = ["A", "B", "C"];
        HashSet<string> hashSet6 = list3.ToHashSet(s => s);

        List<string> otherList = ["A", "B"];
        bool thisIsTrue3 = hashSet6.ContainsRange(otherList);

        // hashset.AddRange(ienumerable)
        List<string> list4 = ["A", "B"];
        HashSet<string> hashSet7 = list4.ToHashSet(s => s);

        List<string> list5 = ["C", "D"];
        hashSet7.AddRange(list5);

        bool thisIsTrue4 = hashSet7.Contains("D");

        // hashset.RemoveRange(ienumerable)
        List<string> list6 = ["A", "B", "C", "D"];
        HashSet<string> hashSet8 = list6.ToHashSet(s => s);

        List<string> list7 = ["A", "B"];
        hashSet8.RemoveRange(list7);

        bool thisIsFalse2 = hashSet8.Contains("A");
        bool thisIsAlsoFalse = hashSet8.Contains("B");
    }
}
