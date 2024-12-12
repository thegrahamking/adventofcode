namespace AdventOfCode.Y2024.Day05;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

[ProblemName("Print Queue")]
class Solution : Solver
{

    public object PartOne(string input)
    {
        var (comparer, updatesToApply) = ParseInput(input);
        return updatesToApply
                    .Where(update => UpdateIsCorrectlyOrdered(update, comparer))
                    .Sum(GetMiddlePageNumber);
    }

    public object PartTwo(string input)
    {
        return 0;
    }

    private static (IComparer<string> comparer, string[][] updatesToApply) ParseInput(string input)
    {
        var parts = input.Split("\n\n");

        var pageOrderingRules = new HashSet<string>(parts[0].Split('\n'));

        var updatesToApply = parts[1].Split('\n').Select(l => l.Split(',')).ToArray();
        return (new OrderRulesComparer(pageOrderingRules), updatesToApply);
    }

    private static bool UpdateIsCorrectlyOrdered(string[] update, IComparer<string> comparer) => update.SequenceEqual(update.Order(comparer));

    private static int GetMiddlePageNumber(string[] u) => int.Parse(u[u.Length / 2]);

    private class OrderRulesComparer : IComparer<string>
    {
        private readonly HashSet<string> orderRules;

        public OrderRulesComparer(HashSet<string> orderRules)
        {
            this.orderRules = orderRules;
        }

        public int Compare(string x, string y)
        {
            return orderRules.Contains($"{x}|{y}") ? -1 : 1;
        }
    }
}