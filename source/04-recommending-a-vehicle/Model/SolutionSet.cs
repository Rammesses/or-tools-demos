using System;
using System.Collections.Generic;
using System.Linq;
using _03_fluent_api.Model;

namespace _04_recommending_a_vehicle.Model
{
    public class SolutionSet : HashSet<SolutionSetItem>, IEnumerable<SolutionSetItem>
    {
        public SolutionSet()
        {
        }

        public void Add(IVehicle vehicle, SolutionRoute initialSolution, SolutionRoute updatedSolution)
        {
            this.Add(new SolutionSetItem(vehicle, initialSolution, updatedSolution));
        }
    }

    public class SolutionSetItem : Tuple<IVehicle, SolutionRoute, SolutionRoute>
    {
        public SolutionSetItem(IVehicle vehicle, SolutionRoute initialSolution, SolutionRoute updatedSolution)
            : base(vehicle, initialSolution, updatedSolution)
        {

        }

        public IVehicle Vehicle => base.Item1;
        public SolutionRoute InitialSolution => base.Item2;
        public SolutionRoute UpdatedSolution => base.Item3;

        public int DifferenceInDistance => UpdatedSolution.Distance - InitialSolution.Distance;
    }

    public static class SolutionSetExtensions
    {
        public static Solution AsSolution(this SolutionSet solutionSet, Func<SolutionSet, SolutionSetItem> optimiser)
        {
            var consideredSolutions = solutionSet.Select(s => s.UpdatedSolution).AsCollection();
            var optimalSolution = optimiser(solutionSet);
            return new Solution(consideredSolutions, optimalSolution.UpdatedSolution);
        }
    }
}
