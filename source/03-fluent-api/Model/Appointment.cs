using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace _03_fluent_api.Model
{
    public interface IAppointment
    {
        ILocation Location {get;}
        IAppointmentWindow Window { get; }
    }


    public class Appointment : IAppointment, ILocation, IComparable<IAppointment>, IComparable
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

        #region IComparable / IComparable<IAppointment> implementation

        public int CompareTo([AllowNull] IAppointment other)
        {
            if (other == null)
            {
                return 1;
            }

            var locationCmp = ((Location)this.Location).CompareTo(other.Location);
            var windowCmp = ((AppointmentWindow)this.Window).CompareTo(other.Window);
            return Math.Max(locationCmp, windowCmp);
        }

        public int CompareTo(object obj)
        {
            var other = obj as IAppointment;
            if (other == null)
                return 1;

            return this.CompareTo(other);
        }

        // Define the is greater than operator.
        public static bool operator >(Appointment operand1, IAppointment operand2)
        {
            return operand1.CompareTo(operand2) == 1;
        }

        // Define the is less than operator.
        public static bool operator <(Appointment operand1, IAppointment operand2)
        {
            return operand1.CompareTo(operand2) == -1;
        }

        // Define the is greater than or equal to operator.
        public static bool operator >=(Appointment operand1, IAppointment operand2)
        {
            return operand1.CompareTo(operand2) >= 0;
        }

        // Define the is less than or equal to operator.
        public static bool operator <=(Appointment operand1, IAppointment operand2)
        {
            return operand1.CompareTo(operand2) <= 0;
        }

        #endregion

        public override string ToString() => $"{this.Postcode} ({this.Window})";
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