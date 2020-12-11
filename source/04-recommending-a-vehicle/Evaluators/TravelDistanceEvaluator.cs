using System;
using _03_fluent_api.Model;
using _03_fluent_api.Services;
using _04_recommending_a_vehicle.Model;
using Google.OrTools.ConstraintSolver;

namespace _04_recommending_a_vehicle.Evaluators
{
    public class TravelDistanceEvaluator
    {
        private readonly IDistanceCalculator distanceCalculator;
        private readonly RoutingIndexManager indexManager;
        private readonly IRoute route;
        
        public TravelDistanceEvaluator(
            RoutingIndexManager indexManager,
            IRoute routeToSolve)
        {
            this.indexManager = indexManager;
            this.route = routeToSolve;

            // we could inject this if we want a different way to calculate
            // distances
            this.distanceCalculator = PostcodeDistanceCalculator.Instance;
        }

        public long MaximumSlack { get; private set; } = TimeOfDay.DayInSeconds;
        public long Capacity { get; private set; } = TimeOfDay.DayInSeconds;

        internal long Run(long t, long u)
        {
            var start = this.indexManager.IndexToNode(t);
            var startLocation = this.route[start];

            var end = this.indexManager.IndexToNode(u);
            var endLocation = this.route[end];

            var distance = this.distanceCalculator.GetDistanceBetween(startLocation, endLocation);
            return distance;
        }
    }

    public static class TravelDistanceEvaluatorExtensions
    {
        public static RecommendationSolver AddElapsedTimeEvaluator(
            this RecommendationSolver model, RoutingIndexManager indexManager, IRoute routeToSolve)
        {
            var evaluator = new TravelDistanceEvaluator(indexManager, routeToSolve);
            var index = model.RegisterTransitCallback(evaluator.Run);
            model.AddDimension(index, evaluator.MaximumSlack, evaluator.Capacity, false, nameof(TravelDistanceEvaluator));
            return model;
        }
    }
}
