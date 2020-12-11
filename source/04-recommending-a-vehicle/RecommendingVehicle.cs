using System;
using _03_fluent_api.Model;
using Google.OrTools.ConstraintSolver;

namespace _04_recommending_a_vehicle
{
    class RecommendingVehicle
    {
        static void Main(string[] args)
        {
            Console.WriteLine("OR-Tools - 04 Recommending a Vehicle");
            var problem = RecommendationModel.Create();

            var appointmentToAdd = Appointment.At("RG5 5EE").Between(TimeOfDay.At("09:00:00"), TimeOfDay.At("17:00:00"));

            var engine = RecommendationEngine.Create(problem);
            var solution = engine.RecommendSolutionFor(appointmentToAdd);

            PrintSolution(problem, solution);
        }

        /// <summary>
        ///   Print the solution.
        /// </summary>
        static void PrintSolution(
            in ProblemModel problem,
            in Solution solution)
        {
            if (solution == null)
            {
                Console.WriteLine("No solution found.");
                return;
            }

            Console.WriteLine($"Vehicle '{solution.OptimalRoute.Vehicle}':");
            Console.Write("Route: ");

            foreach (var location in solution.OptimalRoute)
            {
                Console.WriteLine($"{location}");
            }

            Console.WriteLine($"Route distance: {solution.OptimalRoute.Distance} miles.");
        }
    }
}
