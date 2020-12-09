using System;
using _03_fluent_api.Model;
using Google.OrTools.ConstraintSolver;

namespace _04_recommending_a_vehicle
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("OR-Tools - 04 Recommendations");
            var problem = RecommendationModel.Create();

            var appointmentToAdd = Appointment.At("RG5 5EE").Between(TimeOfDay.At("09:00:00"), TimeOfDay.At("17:00:00"));

            var engine = RecommendationEngine.Create(problem);
            var solution = engine.RecommendSolutionFor(appointmentToAdd);


        }
    }
}
