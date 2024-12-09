namespace AdventOfCode.Y2024.Day04;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;

[ProblemName("Ceres Search")]
class Solution : Solver
{
    static readonly char[] Word = "XMAS".ToCharArray();
    static readonly Vector[] SearchVectors = [new Vector(0, 1), new Vector(1, 1), new Vector(1, 0), new Vector(1, -1), new Vector(0, -1), new Vector(-1, -1), new Vector(-1, 0), new Vector(-1, 1)];

    public object PartOne(string input)
    {
        var wordSearch = ParseInput(input);
        var wordCount = 0;

        for (int i = 0; i < wordSearch[0].Length; i++)
        {
            for (int j = 0; j < wordSearch.Length; j++)
            {
                var start = new Point(i, j);
                foreach (var vector in SearchVectors)
                {
                    var potentialWord = BuildPotentialWord(wordSearch, Word.Length, start, vector);
                    if (potentialWord.SequenceEqual(Word))
                    {
                        wordCount++;
                    }
                }

            }
        }

        return wordCount;
    }

    public object PartTwo(string input)
    {
        return 0;
    }

    private static string[] ParseInput(string input)
    {
        return input.Trim().Split('\n');
    }

    private static char[] BuildPotentialWord(string[] wordSearch, int searchLength, Point start, Vector vector)
    {
        var result = new char[searchLength];
        result[0] = GetLetter(wordSearch, start);
        for (int i = 1; i < searchLength; i++)
        {
            start = start.Travel(vector);
            result[i] = GetLetter(wordSearch, start);
        }

        return result;
    }

    private static char GetLetter(string[] wordSearch, Point start) => start.X < 0 || start.Y < 0 || start.X > wordSearch.Length - 1 || start.Y > wordSearch[0].Length - 1 ? default : wordSearch[start.X][start.Y];

    [DebuggerStepThrough]
    private record Point(int X, int Y)
    {
        public Point Travel(Vector direction)
        {
            return new Point(X + direction.X, Y + direction.Y);
        }
    }

    [DebuggerStepThrough]
    private record Vector(int X, int Y);
}