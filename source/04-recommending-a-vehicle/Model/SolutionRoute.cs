using System;
using System.Collections.Generic;
using _03_fluent_api.Model;
using Google.OrTools.ConstraintSolver;

namespace _04_recommending_a_vehicle.Model
{
    public class SolutionRoute : Route
    {
        public SolutionRoute(
            IRoute solvedRoute,
            int distance)
        {
            SolvedRoute = solvedRoute.Clone();
            Distance = distance;            
        }

        public int Distance { get; }
        public IRoute SolvedRoute { get; }
        public Assignment Solution { get; }
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
