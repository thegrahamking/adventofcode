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

    public object PartOne(string input)
    {
        var reports = input
                        .Split('\n')
                        .Select(r => r.Split(' ').Select(l => int.Parse(l)).ToArray())
                        .ToArray();

        return reports.Count(levels =>
        {
            var pairs = Enumerable.Zip(levels, levels.Skip(1));
            return
                pairs.All(p => p.Second - p.First >= 1 && p.Second - p.First <= 3) ||
                pairs.All(p => p.First - p.Second >= 1 && p.First - p.Second <= 3);
        });
    }


    public object PartTwo(string input)
    {
        return 0;
    }
}