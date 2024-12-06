namespace AdventOfCode.Y2024.Day02;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

[ProblemName("Red-Nosed Reports")]
class Solution : Solver
{

    public object PartOne(string input) => ParseReports(input).Count(IsSafe);

    public object PartTwo(string input) => ParseReports(input).Count(levels => GeneratePotentiallySafeVariations(levels).Any(x => IsSafe(x)));

    private static int[][] ParseReports(string input)
    {
        return input.Split('\n')
                    .Select(r => r.Split(' ').Select(l => int.Parse(l)).ToArray())
                    .ToArray();
    }

    private static int[][] GeneratePotentiallySafeVariations(IReadOnlyCollection<int> levels) =>
        Enumerable
            .Range(0, levels.Count)
            .Select(i =>
            {
                var before = levels.Take(i);
                var after = levels.Skip(i + 1).Take(levels.Count - i - 1);
                return before.Concat(after).ToArray();
            })
            .ToArray();



    private static bool IsSafe(IReadOnlyCollection<int> levels)
    {
        var pairs = Enumerable.Zip(levels, levels.Skip(1));
        return
            pairs.All(p => p.Second - p.First >= 1 && p.Second - p.First <= 3) ||
            pairs.All(p => p.First - p.Second >= 1 && p.First - p.Second <= 3);
    }
}