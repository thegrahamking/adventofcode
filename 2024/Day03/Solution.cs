namespace AdventOfCode.Y2024.Day03;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using AngleSharp.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

[ProblemName("Mull It Over")]
class Solution : Solver
{
    private static readonly Regex mulFinder = new Regex(@"mul\((\d{1,3}),(\d{1,3})\)");

    public object PartOne(string input) => mulFinder.Matches(input).Sum(Multiply);

    public object PartTwo(string input)
    {
        const string enabler = "do()";
        const string disabler = "don't()";

        return
            // Find mul operation, or enablers or disablers
            new Regex(@$"{mulFinder}|{Regex.Escape(enabler)}|{Regex.Escape(disabler)}")
                .Matches(input)
                .Aggregate(
                    // Start off with operations enabled and zero sum
                    (isEnabled: true, total: 0L),
                    (accumulator, match) =>

                        (match.Value, accumulator.isEnabled) switch
                        {
                            // Enabler found - Update the accumulator to enable operations (leave total unchanged)
                            (enabler, _) => (true, accumulator.total),

                            // Disabler found - Update the accumulator to disable operations (leave total unchanged)
                            (disabler, _) => (false, accumulator.total),

                            // Operation found - Update actuator total (leave operations enabled)
                            (_, true) => (true, accumulator.total + Multiply(match)),

                            // Unknown found - Continue
                            _ => accumulator
                        },

                        // Return the accumulated total
                        accumulator => accumulator.total
                    );
    }

    private static int Multiply(Match m)
    {
        return int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value);
    }
}