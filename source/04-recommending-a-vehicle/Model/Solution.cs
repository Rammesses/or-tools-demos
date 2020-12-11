using System;
using System.Collections.Generic;
using _03_fluent_api.Model;
using _04_recommending_a_vehicle.Model;
using Google.OrTools.ConstraintSolver;

namespace _04_recommending_a_vehicle
{
    public class Solution
    {
        public Solution(SolutionRouteCollection consideredRoutes, SolutionRoute optimalRoute)
        {
            ConsideredRoutes = consideredRoutes;
            OptimalRoute = optimalRoute;
        }

        public SolutionRoute OptimalRoute { get; }
        public SolutionRouteCollection ConsideredRoutes { get; }
    }
}
