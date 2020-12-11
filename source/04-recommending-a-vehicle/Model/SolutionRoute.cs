using System;
using System.Collections.Generic;
using _03_fluent_api.Model;
using Google.OrTools.ConstraintSolver;

namespace _04_recommending_a_vehicle.Model
{
    public class SolutionRoute : Route
    {
        public SolutionRoute(
            IRoute routeToSolve,
            Assignment solution,
            Func<Assignment, int> distanceCalculator)
        {
            RouteToSolve = routeToSolve;
            Solution = solution;
            Distance = distanceCalculator(solution);
        }

        public int Distance { get; }
        public IRoute RouteToSolve { get; }
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
