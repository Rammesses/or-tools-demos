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

        IRoute Clone();
    }

    public class Route : IRoute
    {
        public static IRoute For(IVehicle vehicle) => new Route(vehicle);

        public Route(IVehicle vehicle)
        {
            Vehicle = vehicle;
        }

        public ILocation StartLocation { get; private set; }
        public ILocation EndLocation { get; private set; }
        public IVehicle Vehicle { get; }
        public AppointmentCollection Appointments { get; } = new AppointmentCollection();
        public NodeCollection Nodes { get; } = new NodeCollection();

        public LocationCollection Locations => Nodes.Select(n => n.Location).AsCollection();
        public ILocation this[int index] => Nodes[index].Location;

        public void Add(IAppointment appointment)
        {
            this.Appointments.Add(appointment);

            var nodeIndex = (this.EndLocation != null && this.Nodes.Any()) ? this.Nodes.Count - 1 : this.Nodes.Count;
            if (this.EndLocation != null)
            {
                this.Nodes.Insert(nodeIndex, appointment.AsNode(nodeIndex));
                return;
            }

            this.Nodes.Add(appointment.AsNode(nodeIndex));
        }

        public void Add(INode node)
        {
            switch (node)
            {
                case IAppointment appointment:
                    this.Add(appointment);
                    break;
                case ILocation location:
                    this.Add(location);
                    break;
                default:
                    IAppointment nodeAppointment = new Appointment(node.Location, AppointmentWindow.Default);
                    this.Add(nodeAppointment);
                    break;
            }
        }

        public void Add(ILocation location)
        {
            switch (location)
            {
                case IAppointment appointment:
                    this.Add(appointment);
                    break;
                default:
                    IAppointment locationAppointment = new Appointment(location, AppointmentWindow.Default);
                    this.Add(locationAppointment);
                    break;
            }
        }

        internal void SetStartLocation(ILocation startLocation)
        {
            var originalStartLocation = this.StartLocation;
            if (originalStartLocation != null)
            {
                this.Nodes.RemoveAt(0);
            }

            this.StartLocation = startLocation;

            if (startLocation == null)
            {
                return;
            }

            // Use the Vehicle shift-start as the window end
            // i.e. the vehicle is at "home" until shift-start
            var startAppointmentWindow = new AppointmentWindow(
                TimeOfDay.MinValue,
                this.Vehicle.Shift.Start);
            IAppointment startAppointment = new Appointment(
                startLocation,
                startAppointmentWindow);
            this.Nodes.Insert(0, startAppointment.AsNode(0));

            if (this.EndLocation == null)
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

            if (endLocation == null)
            {
                return;
            }

            // Use the Vehicle shift-end as the window start
            // i.e. the vehicle is at "home" after shift-end
            var endAppointmentWindow = new AppointmentWindow(
                this.Vehicle.Shift.End,
                TimeOfDay.MaxValue);

            IAppointment endAppointment = new Appointment(
                endLocation,
                endAppointmentWindow);
            this.Nodes.Add(endAppointment.AsNode(this.Nodes.Count));

            if (this.StartLocation == null)
            {
                this.SetStartLocation(endLocation);
            }    
        }

        public IEnumerator<ILocation> GetEnumerator()
        {
            return this.Nodes.Select(n => n.Location).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Nodes.Select(n => n.Location).GetEnumerator();
        }

        public IRoute Clone()
        {
            var clone = new Route(this.Vehicle);
            clone.StartLocation = this.StartLocation;
            clone.EndLocation = this.EndLocation;

            foreach (var node in this.Nodes)
            {
                clone.Nodes.Add(node);
            }

            return clone;
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
            Console.CursorLeft = 0;
        }        
    }
}
