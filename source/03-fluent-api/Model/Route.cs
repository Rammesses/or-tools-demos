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
        void Add(ILocation location);

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

        public void Add(INode node)
        {
            IAppointment nodeAppointment = new Appointment(node.Location, AppointmentWindow.Default);
            this.Appointments.Add(nodeAppointment);

            var nodeIndex = (this.EndLocation != null) ? this.Nodes.Count - 1 : this.Nodes.Count;
            if (this.EndLocation != null)
            {
                this.Nodes.Insert(nodeIndex, nodeAppointment.AsNode(nodeIndex));
                return;
            }

            this.Nodes.Add(nodeAppointment.AsNode(nodeIndex));
        }

        public void Add(ILocation location)
        {
            this.Appointments.Add(new Appointment(location, AppointmentWindow.Default));
        }

        public NodeCollection Nodes { get; } = new NodeCollection();

        public LocationCollection Locations => Nodes.Select(n => n.Location).AsCollection();

        public ILocation this[int index] => Nodes[index].Location;

        internal void SetStartLocation(ILocation startLocation)
        {
            var originalStartLocation = this.StartLocation;
            if (originalStartLocation != null)
            {
                this.Nodes.RemoveAt(0);
            }

            this.StartLocation = startLocation;

            // this is where you use the Vehicle shift
            IAppointment startAppointment = new Appointment(startLocation, AppointmentWindow.Default);
            this.Nodes.Insert(0, startAppointment.AsNode(0));

            if (this.EndLocation == null || this.EndLocation == originalStartLocation)
            {
                this.SetEndLocation(startLocation);
            }
        }

        internal void SetEndLocation(ILocation endLocation)
        {
            var originalEndLocation = this.EndLocation;
            if (originalEndLocation != null)
            {
                this.Nodes.RemoveAt(this.Nodes.Count - 1);
            }

            this.EndLocation = endLocation;

            IAppointment endAppointment = new Appointment(endLocation, AppointmentWindow.Default);
            this.Nodes.Add(endAppointment.AsNode(this.Nodes.Count));
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
            if (route == null || !route.Any())
            {
                Console.WriteLine("No Route");
                return;
            }

            Console.Write($"{route.Vehicle} :");

            var firstLocation = route.First();
            var lastLocation = route.Last();

            var index = 0;
            foreach (var location in route)
            {
                var separator = location != lastLocation && index++ > 0 ? "=>" : string.Empty;
                Console.Write($"{location} {separator} ");
            }

            Console.WriteLine($" ({route.Nodes.Count} nodes)");
        }

        public static IRoute Clone(this IRoute source)
        {
            var clone = new Route(source.Vehicle);
            clone.SetStartLocation(source.StartLocation);
            clone.SetEndLocation(source.EndLocation);
            foreach (var node in source.Nodes.Where(n => n.Location != source.StartLocation && n.Location != source.EndLocation))
            {
                clone.Add(node);
            }

            return clone;
        }
    }
}
