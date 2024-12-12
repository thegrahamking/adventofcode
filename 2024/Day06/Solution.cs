namespace AdventOfCode.Y2024.Day06;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Reflection.PortableExecutable;

[ProblemName("Guard Gallivant")]
class Solution : Solver
{

    private static readonly IReadOnlyDictionary<char, (char rotated, int xMoveOffset, int yMoveOffset)> guardMovesLookup = new Dictionary<char, (char rotated, int xMoveOffset, int yMoveOffset)>()
    {
        {'^', ('>', 0, 1)},
        {'>', ('v', 1, 0)},
        {'v', ('<', 0, -1)},
        {'<', ('^', -1, 0)}
    };

    public object PartOne(string input)
    {
        var (map, currentGuardPosition) = ParseInput(input);

        var guardPositions = new HashSet<(int, int)>();

        while (PositionIsWithinMap(map, currentGuardPosition))
        {
            // Console.Clear();
            // PrintMap(map);

            guardPositions.Add(currentGuardPosition);
            (map, currentGuardPosition) = Advance(map, currentGuardPosition);
        }

        return guardPositions.Count;
    }

    public object PartTwo(string input)
    {
        var (map, currentGuardPosition) = ParseInput(input);

        var counter = 0;
        var initialGuardPosition = currentGuardPosition;

        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                // Can't use the initial guard position
                if (x == initialGuardPosition.X && y == initialGuardPosition.Y)
                {
                    continue;
                }

                // No point trying existing blocked paths
                if (map[x, y] == '#')
                {
                    continue;
                }

                char[,] testMap = (char[,])map.Clone();
                testMap[x, y] = '#';

                if (PathLoops(testMap, initialGuardPosition))
                {
                    counter++;
                }
            }
        }

        return counter;
    }

    private static bool PathLoops(char[,] map, (int X, int Y) initialGuardPosition)
    {
        var currentGuardPosition = initialGuardPosition;
        var guardStates = new HashSet<(int X, int Y, char position)>();

        while (PositionIsWithinMap(map, currentGuardPosition))
        {
            // Console.Clear();
            // PrintMap(map);

            if (!guardStates.Add((currentGuardPosition.X, currentGuardPosition.Y, map[currentGuardPosition.X, currentGuardPosition.Y])))
            {
                // Guard has been in this state (position and direction) before so we must be in a loop
                return true;
            }

            (map, currentGuardPosition) = Advance(map, currentGuardPosition);
        }

        return false;
    }

    private static void PrintMap(char[,] map)
    {
        for (int i = map.GetLength(1) - 1; i >= 0; i--)
        {
            for (int j = 0; j < map.GetLength(0); j++)
            {
                Console.Write(map[j, i]);
            }

            Console.WriteLine();
        }
    }

    private static (char[,] map, (int X, int Y) guardPosition) Advance(char[,] map, (int X, int Y) currentGuardPosition)
    {

        var guard = map[currentGuardPosition.X, currentGuardPosition.Y];
        var guardMoves = guardMovesLookup[guard];
        var nextGuardPosition = (X: currentGuardPosition.X + guardMoves.xMoveOffset, Y: currentGuardPosition.Y + guardMoves.yMoveOffset);
        char? nextPositionContents = PositionIsWithinMap(map, nextGuardPosition) ?
                                        map[nextGuardPosition.X, nextGuardPosition.Y] :
                                        null;
        if (nextPositionContents.HasValue && nextPositionContents.Equals('#'))
        {
            // Rotate guard
            map[currentGuardPosition.X, currentGuardPosition.Y] = guardMoves.rotated;
            return (map, currentGuardPosition);
        }

        // Move guard
        map[currentGuardPosition.X, currentGuardPosition.Y] = '.';
        if (nextPositionContents.HasValue)
        {
            map[nextGuardPosition.X, nextGuardPosition.Y] = guard;
        }

        return (map, nextGuardPosition);
    }

    private static bool PositionIsWithinMap(char[,] map, (int X, int Y) nextGuardPosition)
    {
        return nextGuardPosition.X >= 0 && nextGuardPosition.X < map.GetLength(0) && nextGuardPosition.Y >= 0 && nextGuardPosition.Y < map.GetLength(1);
    }

    private static (char[,] map, (int X, int Y) guardPosition) ParseInput(string input)
    {
        var lines = input.Split('\n').Reverse().ToArray();

        var map = new char[lines.Length, lines[0].Length];

        (int x, int y)? guardPosition = default;

        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[0].Length; j++)
            {
                var character = lines[i][j];
                map[j, i] = character;

                if (!guardPosition.HasValue && character.Equals('^'))
                {
                    guardPosition = (j, i);
                }
            }
        }

        return (map, guardPosition.Value); ;
    }
}