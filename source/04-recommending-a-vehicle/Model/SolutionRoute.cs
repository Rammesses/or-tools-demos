using System;
using System.Collections;
using System.Collections.Generic;
using _03_fluent_api.Model;
using Google.OrTools.ConstraintSolver;

namespace _04_recommending_a_vehicle.Model
{
    public class SolutionRoute : IRoute
    {
        public SolutionRoute(
            IRoute solvedRoute,
            int distance)
        {
            SolvedRoute = solvedRoute.Clone();
            Distance = distance;            
        }

        public ILocation this[int index] => throw new NotImplementedException();

        public int Distance { get; }
        public IRoute SolvedRoute { get; }
        public Assignment Solution { get; }

        public IVehicle Vehicle => SolvedRoute.Vehicle;

        public ILocation StartLocation => SolvedRoute.StartLocation;

        public ILocation EndLocation => SolvedRoute.EndLocation;

        public NodeCollection Nodes => SolvedRoute.Nodes;

        public void Add(IAppointment appointment)
        {
            throw new NotImplementedException();
        }

        public void Add(ILocation location)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<ILocation> GetEnumerator() => SolvedRoute.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => SolvedRoute.GetEnumerator();
    }

    public class SolutionRouteCollection : HashSet<SolutionRoute>
    {
        public SolutionRouteCollection() : base()
        { }

        public SolutionRouteCollection(IEnumerable<SolutionRoute> source) : base(source)
        { }
    }

    public static class SolutionRouteExtensions
    {
        public static SolutionRouteCollection AsCollection(this IEnumerable<SolutionRoute> source)
        {
            return new SolutionRouteCollection(source);
        }
    }
}
