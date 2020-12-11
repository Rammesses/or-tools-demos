using System;
using System.Diagnostics;
using System.Linq;
using _03_fluent_api.Model;
using _04_recommending_a_vehicle.Evaluators;
using _04_recommending_a_vehicle.Model;
using Google.OrTools.ConstraintSolver;
using Kurukuru;

namespace _04_recommending_a_vehicle
{
    public class RecommendationEngine
    {
        public static RecommendationEngine Create(IProblemModel problem)
        {
            return new RecommendationEngine(problem);
        }

        public RecommendationEngine(IProblemModel problem)
        {
            Problem = problem;
        }

        public IProblemModel Problem { get; }

        public Solution RecommendSolutionFor(IAppointment appointmentToAdd)
        {
            var solutionSet = new SolutionSet();
            foreach (var vehicle in Problem.Vehicles)
            {

                var vehicleRoute = Problem.Routes.For(vehicle);
                vehicleRoute.Print();

                Spinner.Start($"Routing vehicle {vehicle}...", spinner =>
                {
                    var timer = Stopwatch.StartNew();
                    var initialSolution = this.SolveRoute(vehicleRoute);
                    initialSolution.Print();

                    vehicleRoute.Add(appointmentToAdd);
                    var updatedSolution = this.SolveRoute(vehicleRoute);
                    updatedSolution.Print();

                    solutionSet.Add(vehicle, initialSolution, updatedSolution);

                    spinner.Succeed($"Solved initial and updated routes for {vehicle} in {timer.Elapsed}.");
                });
            }

            // choose our recommendation based on the difference in distance metric...
            // i.e. least additional distance to travel
            return solutionSet.AsSolution(s => s.OrderBy(r => r.DifferenceInDistance).First());
        }

        private SolutionRoute SolveRoute(IRoute routeToSolve)
        {
            // the problem route nodes will _always_ have
            // start and end nodes
            var indexManager = new RoutingIndexManager(
                routeToSolve.Nodes.Count,
                1,
                new int[] { 0 },
                new int[] { routeToSolve.Nodes.Count - 1 });

            var parameters = new RoutingModelParameters();

            var solver = new RoutingModel(indexManager, parameters);

            solver.AddElapsedTimeEvaluator(indexManager, routeToSolve);
            //solver.AddAllowedTotalTravelTimeEvaluator(Problem);
            //solver.AddAllowedTravelTimeEvaluator(Problem);
            //solver.AddTravelTimeCostEvaluator(Problem);

            var solution = solver.Solve();

            var solutionRoute = new SolutionRoute(
                routeToSolve,
                solution,
                a => CalculateRouteDistance(indexManager, solver, a));
            return solutionRoute;
        }

        private int CalculateRouteDistance(
            RoutingIndexManager manager,
            RoutingModel routing,
            Assignment solution)
        {
            long routeDistance = 0;
            var index = routing.Start(0);

            while (routing.IsEnd(index) == false)
            {
                var node = manager.IndexToNode((int)index);
                var previousIndex = index;
                index = solution.Value(routing.NextVar(index));

                routeDistance += routing.GetArcCostForVehicle(previousIndex, index, 0);
            }

            return (int)routeDistance;
        }
    }
}