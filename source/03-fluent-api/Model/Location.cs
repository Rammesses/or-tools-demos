using System;

namespace _03_fluent_api.Model
{
    public interface ILocation
    {
        string Postcode { get; }
    }

    public class Location : ILocation
    {
        private string location;

        public Location(string postcode)
        {
            this.Postcode = postcode;
        }

        public string Postcode { get; }
    }

    public static class LocationExtensions
    {
        public static INode AsNode(this ILocation location, int nodeIndex)
        {
            return new Node(location, nodeIndex);
        }
    }
}