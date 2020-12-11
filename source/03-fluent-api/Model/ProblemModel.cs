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
        public virtual VehicleCollection Vehicles { get; protected set; } = new VehicleCollection();

        public virtual RouteCollection Routes { get; protected set; } = new RouteCollection();
    }

}
