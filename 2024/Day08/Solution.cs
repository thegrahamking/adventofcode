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

using Coordinate = (int X, int Y);
using CoordinatePair = ((int X, int Y) First, (int X, int Y) Second);

[ProblemName("Resonant Collinearity")]
class Solution : Solver
{
    public object PartOne(string input)
    {
        var (xMax, yMax, antennas) = ParseInput(input);

        var combinations = antennas.ToDictionary(x => x.Key, x => GetAntennaCombinations(x.Value));

        var antinodeLocations = new HashSet<Coordinate>();

        foreach (CoordinatePair pair in combinations.Values.SelectMany(x => x))
        {
            var antinode1OffSet = (X: pair.First.X - pair.Second.X, Y: pair.First.Y - pair.Second.Y);
            var antinode2OffSet = (X: pair.Second.X - pair.First.X, Y: pair.Second.Y - pair.First.Y);

            var antinode1 = new Coordinate(pair.First.X + antinode1OffSet.X, pair.First.Y + antinode1OffSet.Y);
            var antinode2 = new Coordinate(pair.Second.X + antinode2OffSet.X, pair.Second.Y + antinode2OffSet.Y);

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

    private static bool IsWithinGrid(Coordinate coordinate, int xMax, int yMax)
    {
        return coordinate.X >= 0 && coordinate.X < xMax && coordinate.Y >= 0 && coordinate.Y < yMax;
    }

    private static (Coordinate first, Coordinate second)[] GetAntennaCombinations(List<Coordinate> antennas)
    {
        var comparer = new UniqueCoordinatePairsComparer();
        return antennas
            .SelectMany(x => antennas, (first, second) => new CoordinatePair(first, second))
            .Where(coordinate => coordinate.First != coordinate.Second)
            .Distinct(comparer)
            .ToArray();
    }

    private class UniqueCoordinatePairsComparer : EqualityComparer<CoordinatePair>
    {
        public override bool Equals(CoordinatePair a, CoordinatePair b)
        {
            return (a.First == b.First && a.Second == b.Second) ||
                    (a.First == b.Second && a.Second == b.First);
        }

        public override int GetHashCode([DisallowNull] CoordinatePair obj)
        {
            unchecked
            {
                return obj.First.GetHashCode() * obj.Second.GetHashCode() * 397;
            }
        }
    }

    private static (int xMax, int yMax, Dictionary<char, List<Coordinate>> antennas) ParseInput(string input)
    {
        var lines = input.Split('\n').Reverse().ToArray();

        var xMax = lines[0].Length;
        var yMax = lines.Length;

        var antennas = new Dictionary<char, List<Coordinate>>();

        for (int y = 0; y < yMax; y++)
        {
            for (int x = 0; x < xMax; x++)
            {
                var contents = lines[y][x];
                if (char.IsLetterOrDigit(contents))
                {
                    if (!antennas.TryGetValue(contents, out var locations))
                    {
                        locations = new List<Coordinate>();
                        antennas.Add(contents, locations);
                    }

                    locations.Add((x, y));
                }
            }
        }

        return (xMax, yMax, antennas); ;
    }
}
