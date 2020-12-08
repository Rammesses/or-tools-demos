using System;
using System.Diagnostics;
using Google.OrTools.ConstraintSolver;

namespace _01_simplest_case
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("OR-Tools - 01 Simplest Case");

            // set up the problem
            var problem = new ProblemModel();

            var manager = new RoutingIndexManager(
                problem.DistanceMatrix.GetLength(0),
                problem.Vehicles,
                (int)problem.Depot);

            var routing = new RoutingModel(manager);

            // register the distance callback (and capture its index)
            var transitCallbackIndex = routing.RegisterTransitCallback(
                (long fromIndex, long toIndex) =>
                  {
                      // Convert from routing variable Index to distance matrix NodeIndex.
                      var fromNode = manager.IndexToNode(fromIndex);
                      var fromCity = Enum.GetName(typeof(City), fromNode);

                      var toNode = manager.IndexToNode(toIndex);
                      var toCity = Enum.GetName(typeof(City), fromNode);

                      // pull the correct distance from the matrix
                      var distance = problem.DistanceMatrix[fromNode, toNode];

                      Debug.WriteLine($" - {fromCity} ({fromNode}) -> {toCity} ({toNode}) - {distance} miles");

                      return distance;
                  });

            // TSP rates the "cost" of a route on distance only, so we can use the
            // callback set up above
            routing.SetArcCostEvaluatorOfAllVehicles(transitCallbackIndex);

            var searchParameters = operations_research_constraint_solver.DefaultRoutingSearchParameters();
            searchParameters.FirstSolutionStrategy =
              FirstSolutionStrategy.Types.Value.Automatic;

            var solution = routing.SolveWithParameters(searchParameters);
            PrintSolution(routing, manager, solution);
        }

        /// <summary>
        ///   Print the solution.
        /// </summary>
        static void PrintSolution(
            in RoutingModel routing,
            in RoutingIndexManager manager,
            in Assignment solution)
        {
            Console.WriteLine($"Objective: {solution.ObjectiveValue()} miles");

            Console.Write("Route: ");

            long routeDistance = 0;
            var index = routing.Start(0);

            while (routing.IsEnd(index) == false)
            {
                var node = manager.IndexToNode((int)index);
                var city = Enum.GetName(typeof(City), node);
                Console.Write($"{city} ({node}) -> ");

                var previousIndex = index;
                index = solution.Value(routing.NextVar(index));

                routeDistance += routing.GetArcCostForVehicle(previousIndex, index, 0);
            }

            var lastNode = manager.IndexToNode((int)index);
            var lastCity = Enum.GetName(typeof(City), lastNode);

            Console.WriteLine($"{lastCity} ({lastNode})");
            Console.WriteLine("Route distance: {0}miles", routeDistance);
        }
    }
}
