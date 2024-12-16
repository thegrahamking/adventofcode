namespace AdventOfCode.Y2024.Day08;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.ExceptionServices;
using System.Xml.Serialization;
using System.Reflection.Metadata;

[ProblemName("Resonant Collinearity")]
class Solution : Solver
{

    public object PartOne(string input)
    {
        var (xMax, yMax, antennas) = ParseInput(input);

        var combinations = antennas.ToDictionary(x => x.Key, x => GetAntennaCombinations(x.Value));

        var antinodeLocations = new HashSet<(int x, int y)>();

        foreach (((int x, int y) first, (int x, int y) second) pair in combinations.Values.SelectMany(x => x))
        {
            var antinode1OffSet = (x: pair.first.x - pair.second.x, y: pair.first.y - pair.second.y);
            var antinode2OffSet = (x: pair.second.x - pair.first.x, y: pair.second.y - pair.first.y);

            var antinode1 = (x: pair.first.x + antinode1OffSet.x, y: pair.first.y + antinode1OffSet.y);
            var antinode2 = (x: pair.second.x + antinode2OffSet.x, y: pair.second.y + antinode2OffSet.y);

            if (IsWithinGrid(antinode1, xMax, yMax))
            {
                antinodeLocations.Add(antinode1);
            }

            if (IsWithinGrid(antinode2, xMax, yMax))
            {
                antinodeLocations.Add(antinode2);
            }
        }

        return antinodeLocations.Count;
    }

    public object PartTwo(string input)
    {
        return 0;
    }

    private static bool IsWithinGrid((int x, int y) coordinate, int xMax, int yMax)
    {
        return coordinate.x >= 0 && coordinate.x < xMax && coordinate.y >= 0 && coordinate.y < yMax;
    }

    private static ((int x, int y) first, (int x, int y) second)[] GetAntennaCombinations(List<(int x, int y)> antennas)
    {
        var comparer = new UniqueCoordinatePairsComparer();
        return antennas
            .SelectMany(x => antennas, (first, second) => (first, second))
            .Where(coordinate => coordinate.Item1 != coordinate.Item2)
            .Distinct()
            .ToArray();
    }

    private class UniqueCoordinatePairsComparer : EqualityComparer<((int x, int y), (int x, int y))>
    {
        public override bool Equals(((int x, int y), (int x, int y)) a, ((int x, int y), (int x, int y)) b)
        {
            return (a.Item1 == b.Item1 && a.Item2 == b.Item2) ||
                    (a.Item1 == b.Item2 && a.Item2 == b.Item1);
        }

        public override int GetHashCode([DisallowNull] ((int x, int y), (int x, int y)) pair)
        {
            unchecked
            {
                return pair.Item1.GetHashCode() * pair.Item2.GetHashCode() * 397;
            }
        }

        private static int GetHashCode(int x, int y)
        {
            // https://stackoverflow.com/questions/22826326/good-hashcode-function-for-2d-coordinates
            int tmp = y + ((x + 1) / 2);
            return x + (tmp * tmp);
        }
    }

    private static (int xMax, int yMax, Dictionary<char, List<(int X, int Y)>> antennas) ParseInput(string input)
    {
        var lines = input.Split('\n').Reverse().ToArray();

        var xMax = lines[0].Length;
        var yMax = lines.Length;

        var antennas = new Dictionary<char, List<(int X, int Y)>>();

        for (int y = 0; y < yMax; y++)
        {
            for (int x = 0; x < xMax; x++)
            {
                var contents = lines[y][x];
                if (char.IsLetterOrDigit(contents))
                {
                    if (!antennas.TryGetValue(contents, out var locations))
                    {
                        locations = new List<(int X, int Y)>();
                        antennas.Add(contents, locations);
                    }

                    locations.Add((x, y));
                }
            }
        }

        return (xMax, yMax, antennas); ;
    }
}