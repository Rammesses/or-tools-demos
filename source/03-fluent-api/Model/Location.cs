using System.Collections.Generic;

namespace _03_fluent_api.Model
{
    public interface ILocation
    {
        string Postcode { get; }
    }

    public class Location : ILocation
    {
        public Location(string postcode)
        {
            this.Postcode = postcode;
        }

        public string Postcode { get; }

        public override string ToString() => this.Postcode;
    }

    public class LocationCollection : HashSet<ILocation>
    {
        public LocationCollection() : base()
        { }

        public LocationCollection(IEnumerable<ILocation> source) : base(source)
        { }
    }

    public static class LocationExtensions
    {
        public static INode AsNode(this ILocation location, int nodeIndex)
        {
            return new Node(location, nodeIndex);
        }

        public static LocationCollection AsCollection(this IEnumerable<ILocation> source)
        {
            return new LocationCollection(source);
        }
    }
}