using System;
using _03_fluent_api.Model;

namespace _03_fluent_api
{
    public static class FluentAPI
    {
        public static IProblemVehiclePair AddVehicleRoute(this IProblemModel problem, string vehicleId)
        {
            var vehicle = new Vehicle(vehicleId);
            problem.Vehicles.Add(vehicle);
            problem.Routes.Add(Route.For(vehicle));
            return new ProblemVehiclePair(problem, vehicle);
        }

        public static IProblemVehiclePair LeavingFrom(this IProblemVehiclePair pair, string postcode)
        {
            var route = (Route)pair.Problem.Routes.For(pair.Vehicle);
            route.SetStartLocation(new Location(postcode));
            return pair;
        }

        public static IProblemVehiclePair WithShift(this IProblemVehiclePair pair, IShift shift)
        {
            ((Vehicle)pair.Vehicle).SetShift(shift);
            return pair;
        }

        public static IProblemVehiclePair WithAppointment(this IProblemVehiclePair pair, IAppointment appointment)
        {
            pair.Problem.Routes.For(pair.Vehicle).Add(appointment);
            return pair;
        }

        public static IProblemVehiclePair ReturningToStart(this IProblemVehiclePair pair)
        {
            var route = (Route)pair.Problem.Routes.For(pair.Vehicle);
            route.SetEndLocation(route.StartLocation);
            return pair;
        }

        public static IProblemVehiclePair ReturningTo(this IProblemVehiclePair pair, string postcode)
        {
            var route = (Route)pair.Problem.Routes.For(pair.Vehicle);
            route.SetEndLocation(new Location(postcode));
            return pair;
        }
    }
}
