namespace AdventOfCode.Y2024.Day01;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

[ProblemName("Historian Hysteria")]
class Solution : Solver
{

    public object PartOne(string input) =>
        Enumerable.Zip(GetOrderedColumn(input, 0), GetOrderedColumn(input, 1))
            .Select(p => Math.Abs(p.First - p.Second))
            .Sum();

    public object PartTwo(string input)
    {
        var left = GetColumn(input, 0);
        var rightMap = GetColumn(input, 1).CountBy(x => x).ToDictionary();

        var similarityScore = left.Sum(x => rightMap.TryGetValue(x, out var result) ? x * result : 0);

        return similarityScore;
    }

    private IReadOnlyCollection<int> GetColumn(string input, int columnIndex)
    {
        var lines = input.Split('\n');
        var numbers = lines.Select(l => int.Parse(l.Split("   ")[columnIndex]));
        return numbers.ToArray();
    }
    
    private IReadOnlyCollection<int> GetOrderedColumn(string input, int columnIndex)
    {
        return GetColumn(input, columnIndex).Order().ToArray();
    }
}