using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace _03_fluent_api.Model
{
    public interface ILocation
    {
        string Postcode { get; }
    }

    public class Location : ILocation, IComparable<ILocation>, IComparable
    {
        public Location(string postcode)
        {
            this.Postcode = postcode;
        }

        public string Postcode { get; }

        #region IComparable / IComparable<ILocation> implementation

        public int CompareTo([AllowNull] ILocation other)
        {
            if (other == null)
            {
                return 1;
            }

            return string.Compare(this.Postcode, other.Postcode);
        }

        public int CompareTo(object obj)
        {
            var other = obj as INode;
            if (other == null)
                return 1;

            return this.CompareTo(other);
        }

        // Define the is greater than operator.
        public static bool operator >(Location operand1, ILocation operand2)
        {
            return operand1.CompareTo(operand2) == 1;
        }

        // Define the is less than operator.
        public static bool operator <(Location operand1, ILocation operand2)
        {
            return operand1.CompareTo(operand2) == -1;
        }

        // Define the is greater than or equal to operator.
        public static bool operator >=(Location operand1, ILocation operand2)
        {
            return operand1.CompareTo(operand2) >= 0;
        }

        // Define the is less than or equal to operator.
        public static bool operator <=(Location operand1, ILocation operand2)
        {
            return operand1.CompareTo(operand2) <= 0;
        }

        #endregion


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