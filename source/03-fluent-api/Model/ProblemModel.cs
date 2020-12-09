using System;

namespace _03_fluent_api.Model
{
    public interface IProblemModel
    {
        VehicleCollection Vehicles { get; }
        RouteCollection Routes { get; }
    }

    public abstract class ProblemModel : IProblemModel
    {
        public VehicleCollection Vehicles => throw new NotImplementedException();

        public RouteCollection Routes => throw new NotImplementedException();
    }

}
