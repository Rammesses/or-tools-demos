using System;
using System.Collections.Generic;
using System.Linq;

namespace _03_fluent_api.Model
{
    public interface IRoute
    {
        IVehicle Vehicle { get; }
        ILocation StartLocation { get; }
        ILocation EndLocation { get;  }
        NodeCollection Nodes { get; }

        void Add(IAppointment appointment);
    }

    public class Route : IRoute
    {
        public IVehicle Vehicle { get; }
        public AppointmentCollection Appointments { get; } = new AppointmentCollection();

        public static IRoute For(IVehicle vehicle) => new Route(vehicle);

        public Route()
        {
        }

        public Route(IVehicle vehicle)
        {
            Vehicle = vehicle;
        }

        public ILocation StartLocation { get; private set; }
        public ILocation EndLocation { get; private set; }
        public NodeCollection Nodes { get; } = new NodeCollection();

        public void Add(IAppointment appointment)
        {
            this.Appointments.Add(appointment);
        }

        public NodeCollection Node => GetNodes();

        internal NodeCollection GetNodes()
        {
            var nodes = new NodeCollection();
            nodes.Add(StartLocation.AsNode(0));

            var index = 0;
            foreach (var appointment in this.Appointments)
            {
                nodes.Add(appointment.AsNode(++index));
            }

            var endNode = (EndLocation != StartLocation) ?
                EndLocation.AsNode(nodes.Count) :
                StartLocation.AsNode(nodes.Count);

            return nodes;
        }

        internal void SetStartLocation(ILocation startLocation)
        {
            this.StartLocation = startLocation;
        }

        internal void SetEndLocation(ILocation endLocation)
        {
            this.EndLocation = endLocation;
        }
    }

    public class RouteCollection : HashSet<IRoute>
    {
        public IRoute For(IVehicle vehicle) => this.SingleOrDefault(r => r.Vehicle == vehicle);
    }
}
