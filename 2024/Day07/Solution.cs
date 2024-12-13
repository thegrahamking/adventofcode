namespace AdventOfCode.Y2024.Day07;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

[ProblemName("Bridge Repair")]
class Solution : Solver
{

    public object PartOne(string input)
    {
        var operations = ParseInput(input);

        return operations
            .Where(o => EquationIsValid(o.TestValue, o.Numbers[0], o.Numbers[1..]))
            .Sum(o => o.TestValue);
    }

    public object PartTwo(string input)
    {
        return 0;
    }

    private static (long TestValue, long[] Numbers)[] ParseInput(string input)
    {
        return input
            .Split('\n')
            .Select(l =>
             {
                 var parts = l.Split(':');
                 var testValue = long.Parse(parts[0]);
                 var numbers = parts[1].Trim().Split(' ').Select(long.Parse).ToArray();
                 return (testValue, numbers);
             })
            .ToArray();
    }

    private static bool EquationIsValid(long testValue, long currentTotal, long[] numbers)
    {
        return numbers.Length == 0
            ? currentTotal == testValue
            : EquationIsValid(testValue, currentTotal * numbers[0], numbers[1..]) || EquationIsValid(testValue, currentTotal + numbers[0], numbers[1..]);
    }
}