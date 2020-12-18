using System;
using System.Linq;
using _03_fluent_api.Model;
using _04_recommending_a_vehicle.Model;
using Google.OrTools.ConstraintSolver;

namespace _04_recommending_a_vehicle
{
    class RecommendingVehicle
    {
        static void Main(string[] args)
        {
            Console.WriteLine("OR-Tools - 04 Recommending a Vehicle\n");
            var problem = RecommendationModel.Create();

            var appointmentToAdd = Appointment.At("RG10 10HH").Between(TimeOfDay.At("09:00:00"), TimeOfDay.At("17:00:00"));

            var engine = RecommendationEngine.Create(problem);
            var solutions = engine.RecommendSolutionsFor(appointmentToAdd);

            PrintSolutions(problem, solutions);
        }

        /// <summary>
        ///   Print the solution.
        /// </summary>
        static void PrintSolutions(
            in ProblemModel problem,
            in SolutionSet solutions)
        {
            Console.WriteLine("\nConsidered solutions for:");
            foreach (var considered in solutions)
            {
                Console.WriteLine($" - {considered.Vehicle} - {considered.DifferenceInDistance} additional miles");
            }

            Console.WriteLine("\nOptimal Solution:");
            var optimal = solutions.AsSolution(s => s.OrderBy(r => r.DifferenceInDistance).First());
            optimal.OptimalRoute.Print();
            Console.WriteLine($"Route distance: {optimal.OptimalRoute.Distance} miles.");
        }
    }
}
