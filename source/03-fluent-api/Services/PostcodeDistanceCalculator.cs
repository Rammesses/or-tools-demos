using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using _03_fluent_api.Model;

namespace _03_fluent_api.Services
{
    public class PostcodeDistanceCalculator : IDistanceCalculator
    {
        public static PostcodeDistanceCalculator Instance = new PostcodeDistanceCalculator();

        private PostcodeDistanceCalculator()
        {
        }

        public int GetDistanceBetween(ILocation start, ILocation end)
        {
            return GetDistanceBetween(start.Postcode, end.Postcode);
        }

        public int GetDistanceBetween(string start, string end)
        {
            var startCoords = GetEquivalentCoordinates(start);
            var endCoords = GetEquivalentCoordinates(end);

            var distance = GetManhattanDistance(startCoords, endCoords);

            Debug.WriteLine($"{start} {startCoords} => {end} {endCoords} = {distance}");

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

            var xVal = match.Groups[1].Value;
            var yVal = match.Groups[2].Value;
            var x = Convert.ToInt32(xVal);
            var y = Convert.ToInt32(yVal);
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

            public override string ToString() => $"({X}, {Y})";
        }
    }
}
