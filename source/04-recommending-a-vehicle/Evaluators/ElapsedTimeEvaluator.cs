using System;
using _03_fluent_api.Model;
using Google.OrTools.ConstraintSolver;

namespace _04_recommending_a_vehicle.Evaluators
{
    public class ElapsedTimeEvaluator
    {
        public ElapsedTimeEvaluator(IProblemModel problem)
        {
        }

        public long MaximumSlack { get; private set; } = TimeOfDay.DayInSeconds;
        public long Capacity { get; private set; } = TimeOfDay.DayInSeconds;

        internal long Run(long t, long u)
        {
            throw new NotImplementedException();
        }
    }

    public static class ElapsedTimeEvaluatorExtensions
    {
        public static RoutingModel AddElapsedTimeEvaluator(this RoutingModel model, IProblemModel problem)
        {
            var evaluator = new ElapsedTimeEvaluator(problem);
            var index = model.RegisterTransitCallback(evaluator.Run);
            model.AddDimension(index, evaluator.MaximumSlack, evaluator.Capacity, false, nameof(ElapsedTimeEvaluator));
            return model;
        }
    }
}
