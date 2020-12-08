using System;
using System.Diagnostics;
using Google.OrTools.ConstraintSolver;

namespace _02_multiple_vehicles
{
    class Program
    {
        const string dimensionName = @"Minimise Vehicle Distance";
        const int maximumSlackPerVehicle = 0;
        const int maximumDistancePerVehicle = 3000; // maximum distance per vericle
        const bool startCumulativeCalculationAtZeroPerVehicle = true; // why wouldn't you start at 0?!
        const int costCoefficient = 100; // makes the solver minimise the length of the longest route

        static void Main(string[] args)
        {
            Console.WriteLine("OR-Tools - 02 Multiple Vehicles");

            // set up the problem
            var problem = new ProblemModel();

            var manager = new RoutingIndexManager(
                problem.NumberOfCities,
                problem.Vehicles,
                (int)problem.Depot);

            var routing = new RoutingModel(manager);

            // register the distance callback (and capture its index)
            var transitCallbackIndex = routing.RegisterTransitCallback(
                (long fromIndex, long toIndex) =>
                  {
                      // Convert from routing variable Index to distance matrix NodeIndex.
                      var fromNode = manager.IndexToNode(fromIndex);
                      var toNode = manager.IndexToNode(toIndex);

                      // pull the correct distance from the matrix
                      var distance = problem.DistanceMatrix[fromNode, toNode];
                      return distance;
                  });

            // TSP rates the "cost" of a route on distance only, so we can use the
            // callback set up above
            routing.SetArcCostEvaluatorOfAllVehicles(transitCallbackIndex);

            // Add a Dimension to minimise the distance _for each vehicle_
            routing.AddDimension(
                transitCallbackIndex, 
                maximumSlackPerVehicle, 
                maximumDistancePerVehicle,
                startCumulativeCalculationAtZeroPerVehicle,
                dimensionName);

            RoutingDimension distanceDimension = routing.GetMutableDimension(dimensionName);
            distanceDimension.SetGlobalSpanCostCoefficient(costCoefficient);

            var searchParameters = operations_research_constraint_solver.DefaultRoutingSearchParameters();
            searchParameters.FirstSolutionStrategy =
              FirstSolutionStrategy.Types.Value.PathCheapestArc;

            var solution = routing.SolveWithParameters(searchParameters);
            PrintSolution(problem, routing, manager, solution);
        }

        /// <summary>
        ///   Print the solution.
        /// </summary>
        static void PrintSolution(

            in ProblemModel problem,
            in RoutingModel routing,
            in RoutingIndexManager manager,
            in Assignment solution)
        {

            long maxRouteDistance = 0;
            for (int vehicleIndex = 0; vehicleIndex < problem.Vehicles; ++vehicleIndex)
            {
                Console.Write($"Route for Vehicle #{vehicleIndex}: ");

                long routeDistance = 0;

                var index = routing.Start(vehicleIndex);
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

                Console.WriteLine($"Route distance for Vehicle #{vehicleIndex} : {routeDistance} miles");
                maxRouteDistance = Math.Max(routeDistance, maxRouteDistance);
            }

            Console.WriteLine("Maximum distance of any route: {0}m", maxRouteDistance);        
        }
    }
}
