using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace _03_fluent_api.Model
{
    public interface IRoute : IEnumerable<ILocation>
    {
        IVehicle Vehicle { get; }
        ILocation StartLocation { get; }
        ILocation EndLocation { get;  }
        NodeCollection Nodes { get; }

        void Add(IAppointment appointment);

        ILocation this[int index]
        {
            get;
        }
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

        public void Add(IAppointment appointment)
        {
            this.Appointments.Add(appointment);
        }

        public NodeCollection Nodes => GetNodes();
        public LocationCollection Locations => Nodes.Select(n => n.Location).AsCollection();

        public ILocation this[int index] => Nodes[index].Location;

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

        public IEnumerator<ILocation> GetEnumerator()
        {
            return this.Nodes.Select(n => n.Location).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Nodes.Select(n => n.Location).GetEnumerator();
        }
    }

    public class RouteCollection : HashSet<IRoute>
    {
        public IRoute For(IVehicle vehicle) => this.SingleOrDefault(r => r.Vehicle == vehicle);
    }

    public static class RouteExtensions
    {
        public static void Print(this IRoute route)
        {
            Console.Write($"{route.Vehicle} :");
            foreach (var location in route)
            {
                var separator = (location != route.EndLocation) ?
                    " => " : string.Empty;
                Console.Write($"{location} {separator}");
            }

            Console.WriteLine($" ({route.Nodes.Count} nodes)");
        }
    }
}
