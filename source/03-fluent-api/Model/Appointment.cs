using System.Collections.Generic;

namespace _03_fluent_api.Model
{
    public interface IAppointment
    { }

    public class Appointment : IAppointment, ILocation
    {
        public static IAppointment At(string location) => new Appointment(new Location(location), AppointmentWindow.Default);

        public Appointment()
        {}

        public Appointment(ILocation location, IAppointmentWindow window)
        {
            this.Location = location;
            this.Window = window;
        }

        public ILocation Location { get; private set; }
        public IAppointmentWindow Window { get; private set; }

        public string Postcode => this.Location?.Postcode;

        internal void SetWindow(AppointmentWindow window) => this.Window = window;
    }

    public class AppointmentCollection : HashSet<IAppointment>
    { }

    public static class AppointmentExtensions
    {
        public static IAppointment Within(this IAppointment source, AppointmentWindow window)
        {
            ((Appointment)source).SetWindow(window);
            return source;
        }

        public static IAppointment Between(this IAppointment source, TimeOfDay start, TimeOfDay end)
        {
            ((Appointment)source).SetWindow(new AppointmentWindow(start, end));
            return source;
        }

        public static INode AsNode(this IAppointment source, int index)
        {
            return new Node((ILocation)source, index);
        }
    }
}