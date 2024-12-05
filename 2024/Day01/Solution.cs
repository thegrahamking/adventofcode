namespace AdventOfCode.Y2024.Day01;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

[ProblemName("Historian Hysteria")]
class Solution : Solver {

    public object PartOne(string input) =>
        Enumerable.Zip(GetOrderedColumn(input, 0),GetOrderedColumn(input, 1))
            .Select(p=> Math.Abs(p.First - p.Second))
            .Sum();

    public object PartTwo(string input) {
        return 0;
    }

    private IReadOnlyCollection<int> GetOrderedColumn(string input, int columnIndex)
    {
        var lines = input.Split('\n');
        var numbers = lines.Select(l=> int.Parse(l.Split("   ")[columnIndex]));
        return numbers.Order().ToArray();
    }
}