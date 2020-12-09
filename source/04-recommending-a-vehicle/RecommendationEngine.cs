using System;
using _03_fluent_api.Model;
using _04_recommending_a_vehicle.Evaluators;
using Google.OrTools.ConstraintSolver;

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
            foreach (var vehicle in Problem.Vehicles)
            {
                var initialRoute = Problem.Routes.For(vehicle);

                // the problem route nodes will _always_ have
                // start and end nodes
                var indexManager = new RoutingIndexManager(
                    initialRoute.Nodes.Count,
                    1,
                    new int[] { 0 },
                    new int[] { initialRoute.Nodes.Count - 1 });

                var parameters = new RoutingModelParameters();

                var solver = new RoutingModel(indexManager, parameters);

                solver.AddElapsedTimeEvaluator(Problem);
                //solver.AddAllowedTotalTravelTimeEvaluator(Problem);
                //solver.AddAllowedTravelTimeEvaluator(Problem);
                //solver.AddTravelTimeCostEvaluator(Problem);

                var solution = solver.Solve();
            }

            return null;
        }
    }
}