using System;
using _03_fluent_api.Model;
using Google.OrTools.ConstraintSolver;

namespace _04_recommending_a_vehicle
{
    public class Solution : Tuple<Vehicle, Assignment>
    {
        public Solution(Vehicle item1, Assignment item2) : base(item1, item2)
        {
        }
    }
}
