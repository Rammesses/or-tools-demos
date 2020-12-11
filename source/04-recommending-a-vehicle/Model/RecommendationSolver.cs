using System;
using System.Collections.Concurrent;
using Google.OrTools.ConstraintSolver;

namespace _04_recommending_a_vehicle.Model
{
    public class RecommendationSolver : RoutingModel
    {
        private readonly ConcurrentDictionary<string, int> EvaluatorMap = new ConcurrentDictionary<string, int>();

        public RecommendationSolver(RoutingIndexManager index_manager) : base(index_manager)
        {
        }

        public RecommendationSolver(RoutingIndexManager index_manager, RoutingModelParameters parameters) : base(index_manager, parameters)
        {
        }

        public int GetEvaluatorIndex(string evaluatorName)
        {
            return EvaluatorMap.ContainsKey(evaluatorName) ?
                EvaluatorMap[evaluatorName] : -1;
        }

        public new bool AddDimension(int evaluator_index, long slack_max, long capacity, bool fix_start_cumul_to_zero, string name)
        {
            EvaluatorMap.AddOrUpdate(name, evaluator_index, (n, i) => evaluator_index);
            return base.AddDimension(evaluator_index, slack_max, capacity, fix_start_cumul_to_zero, name);
        }
    }
}
