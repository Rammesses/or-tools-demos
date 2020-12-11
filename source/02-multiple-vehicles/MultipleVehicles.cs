using System;
using System.Diagnostics;
using Google.OrTools.ConstraintSolver;
using Google.Protobuf.WellKnownTypes;
using Kurukuru;

namespace _02_multiple_vehicles
{
    class MultipleVehicles
    {
        static void Main(string[] args)
        {
            Console.WriteLine("OR-Tools - 02 Multiple Vehicles (VRP)");

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
                    var fromCity = System.Enum.GetName(typeof(City), fromNode);

                    var toNode = manager.IndexToNode(toIndex);
                    var toCity = System.Enum.GetName(typeof(City), fromNode);

                    // pull the correct distance from the matrix
                    var distance = problem.DistanceMatrix[fromNode, toNode];

                    Debug.WriteLine($" - {fromCity} ({fromNode}) -> {toCity} ({toNode}) - {distance} miles");

                    return distance;
                });

            // TSP rates the "cost" of a route on distance only, so we can use the
            // callback set up above
            routing.SetArcCostEvaluatorOfAllVehicles(transitCallbackIndex);

            routing.AddDimension(transitCallbackIndex, 0, 3000,true, "Distance");
            var distanceDimension = routing.GetMutableDimension("Distance");
            distanceDimension.SetGlobalSpanCostCoefficient(100);

            var searchParameters = operations_research_constraint_solver.DefaultRoutingSearchParameters();
            searchParameters.FirstSolutionStrategy =
              FirstSolutionStrategy.Types.Value.Automatic;
            searchParameters.TimeLimit = new Duration { Seconds = 60 };

            Assignment solution = null;
            Spinner.Start($"Solving VRP for {problem.Vehicles} vehicle(s)...", spinner =>
            {
                var timer = Stopwatch.StartNew();
                solution = routing.SolveWithParameters(searchParameters);

                if (solution != null)
                {
                    spinner.Succeed($"Solved VRP  for {problem.Vehicles} vehicle(s) in {timer.Elapsed}.");
                }
                else
                {
                    spinner.Fail($"Failed to find a VRP solution for {problem.Vehicles} vehicle(s) after {timer.Elapsed}");
                }
            });

            PrintSolution(problem, routing, manager, solution);
        }

        /// <summary>
        ///   Print the solution.
        /// </summary>
        static void PrintSolution(
            in ProblemModel model,
            in RoutingModel routing,
            in RoutingIndexManager manager,
            in Assignment solution)
        {
            if (solution == null)
            {
                Console.WriteLine("No solution found.");
                return;
            }

            long maxRouteDistance = 0;
            for (int i = 0; i < model.Vehicles; ++i)
            {
                Console.WriteLine("\nRoute for Vehicle {0}:", i);
                long routeDistance = 0;
                var index = routing.Start(i);
                while (routing.IsEnd(index) == false)
                {
                    var node = manager.IndexToNode((int)index);
                    var city = System.Enum.GetName(typeof(City), node);
                    Console.Write($"{city} ({node}) -> ");

                    var previousIndex = index;
                    index = solution.Value(routing.NextVar(index));
                    routeDistance += routing.GetArcCostForVehicle(previousIndex, index, 0);
                }

                var lastNode = manager.IndexToNode((int)index);
                var lastCity = System.Enum.GetName(typeof(City), lastNode);

                Console.WriteLine($"{lastCity} ({lastNode})");
                Console.WriteLine("Distance of the route: {0}m", routeDistance);
                maxRouteDistance = Math.Max(routeDistance, maxRouteDistance);
            }

            Console.WriteLine("\nMaximum distance of any route: {0}m", maxRouteDistance);
        }
    }
}
