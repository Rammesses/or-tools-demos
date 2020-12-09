using System;
using System.Text.RegularExpressions;

namespace _03_fluent_api.Services
{
    public class PostcodeDistanceCalculator
    {
        public static PostcodeDistanceCalculator Instance = new PostcodeDistanceCalculator();

        private PostcodeDistanceCalculator()
        {
        }

        public int GetDistanceBetween(string start, string end)
        {
            var startCoords = GetEquivalentCoordinates(start);
            var endCoords = GetEquivalentCoordinates(end);

            var distance = GetManhattanDistance(startCoords, endCoords);
            return distance;
        }

        private static Regex ExtractCoords = new Regex(@"\D*(\d+)\s+(\d+)\D*");

        public Coord GetEquivalentCoordinates(string postcode)
        {
            var match = ExtractCoords.Match(postcode);
            if (!match.Success)
            {
                throw new ArgumentException($"{postcode} isn't a postcode!");
            }

            var x = Convert.ToInt32(match.Captures[0]);
            var y = Convert.ToInt32(match.Captures[1]);
            return new Coord(x, y);
        }

        public int GetManhattanDistance(Coord start, Coord end)
        {
            return (start.X - end.X) + (start.Y + end.Y);
        }

        public struct Coord
        {
            public Coord(int x, int y)
            {
                X = x;
                Y = y;
            }

            public int X { get; }
            public int Y { get; }
        }
    }
}
