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
    // X and Y coordinates are reversed - Need sleep, no time to fix

    public object PartOne(string input)
    {
        var wordSearch = ParseInput(input);
        var word = "XMAS".ToCharArray();
        var wordCount = 0;

        Vector[] searchVectors = [new Vector(0, 1), new Vector(1, 1), new Vector(1, 0), new Vector(1, -1), new Vector(0, -1), new Vector(-1, -1), new Vector(-1, 0), new Vector(-1, 1)];

        for (int i = 0; i < wordSearch[0].Length; i++)
        {
            for (int j = 0; j < wordSearch.Length; j++)
            {
                var start = new Point(i, j);
                foreach (var vector in searchVectors)
                {
                    var potentialWord = GetWordFromPointOnVector(wordSearch, word.Length, start, vector);
                    if (potentialWord.SequenceEqual(word))
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
        var wordSearch = ParseInput(input);
        var mas = "MAS".ToCharArray();
        var wordCount = 0;

        // Can start from 1 and exclude last in both axis because cross cannot be centered on outer edge
        for (int i = 1; i < wordSearch.Length - 1; i++)
        {
            for (int j = 1; j < wordSearch[0].Length - 1; j++)
            {
                var start = new Point(j, i);
                var topLeftToBottomRight = GetWordFromPointOnVector(wordSearch, mas.Length, start.Travel(new Vector(-1, -1)), new Vector(1, 1));
                var topRightToBottomLeft = GetWordFromPointOnVector(wordSearch, mas.Length, start.Travel(new Vector(-1, 1)), new Vector(1, -1));
                
                // Could have done this with vectors but this works too
                var bottomRightToTopLeft = topLeftToBottomRight.Reverse().ToArray();
                var bottomLeftToTopRight = topRightToBottomLeft.Reverse().ToArray();

                if ((topLeftToBottomRight.SequenceEqual(mas) && topRightToBottomLeft.SequenceEqual(mas)) ||
                    (bottomRightToTopLeft.SequenceEqual(mas) && bottomLeftToTopRight.SequenceEqual(mas)) ||
                    (topLeftToBottomRight.SequenceEqual(mas) && bottomLeftToTopRight.SequenceEqual(mas)) ||
                    (bottomRightToTopLeft.SequenceEqual(mas) && topRightToBottomLeft.SequenceEqual(mas)) ||
                    (bottomRightToTopLeft.SequenceEqual(mas) && bottomLeftToTopRight.SequenceEqual(mas)))
                {
                    wordCount++;
                }

            }
        }

        return wordCount;

    }

    private static string[] ParseInput(string input)
    {
        return input.Trim().Split('\n');
    }

    private static char[] GetWordFromPointOnVector(string[] wordSearch, int searchLength, Point start, Vector vector)
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