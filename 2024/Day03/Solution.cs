namespace AdventOfCode.Y2024.Day03;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

[ProblemName("Mull It Over")]
class Solution : Solver
{
    private static readonly Regex regex = new Regex(@"mul\((\d{1,3}),(\d{1,3})\)");

    public object PartOne(string input)
    {
        return regex.Matches(input).Sum(m => int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value));
    }

    public object PartTwo(string input)
    {
        return 0;
    }
}