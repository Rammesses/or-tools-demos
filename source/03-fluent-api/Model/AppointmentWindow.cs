using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace _03_fluent_api.Model
{
    public interface IAppointmentWindow
    {
        TimeOfDay Start { get; }
        TimeOfDay End { get; }
    }

    public class AppointmentWindow : IAppointmentWindow, IComparable<IAppointmentWindow>, IComparable, IEquatable<AppointmentWindow>
    {
        public static IAppointmentWindow Default = new AppointmentWindow(TimeOfDay.At("07:00:00"), TimeOfDay.At("22:00:00"));
        public static IAppointmentWindow StartingAt(TimeOfDay start) => new AppointmentWindow(start, Default.End);
        public static IAppointmentWindow EndingAt(TimeOfDay end) => new AppointmentWindow(Default.Start, end);

        public AppointmentWindow() : this(AppointmentWindow.Default)
        {
        }

        public AppointmentWindow(IAppointmentWindow source)
            : this(source.Start, source.End)
        { }

        public AppointmentWindow(TimeOfDay start, TimeOfDay end)
        {
            if (end <= start)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(end),
                    $"Shift end ({end}) must be after (and not same as) start ({start})");
            }

            this.Start = start;
            this.End = end;
        }

        public TimeOfDay Start { get; }
        public TimeOfDay End { get; }

        #region IComparable / IComparable<IAppointmentWindow> implementation

        public int CompareTo([AllowNull] IAppointmentWindow other)
        {
            if (other == null)
            {
                return 1;
            }

            var start = this.Start.CompareTo(other.Start);
            var end = this.End.CompareTo(other.End);
            return Math.Max(start, end);
        }

        public int CompareTo(object obj)
        {
            var other = obj as IAppointmentWindow;
            if (other == null)
                return 1;

            return this.CompareTo(other);
        }

        // Define the is greater than operator.
        public static bool operator >(AppointmentWindow operand1, IAppointmentWindow operand2)
        {
            return operand1.CompareTo(operand2) == 1;
        }

        // Define the is less than operator.
        public static bool operator <(AppointmentWindow operand1, IAppointmentWindow operand2)
        {
            return operand1.CompareTo(operand2) == -1;
        }

        // Define the is greater than or equal to operator.
        public static bool operator >=(AppointmentWindow operand1, IAppointmentWindow operand2)
        {
            return operand1.CompareTo(operand2) >= 0;
        }

        // Define the is less than or equal to operator.
        public static bool operator <=(AppointmentWindow operand1, IAppointmentWindow operand2)
        {
            return operand1.CompareTo(operand2) <= 0;
        }

        public static bool operator ==(AppointmentWindow operand1, IAppointmentWindow operand2)
        {
            return operand1.CompareTo(operand2) == 0;
        }

        public static bool operator !=(AppointmentWindow operand1, IAppointmentWindow operand2)
        {
            return operand1.CompareTo(operand2) != 0;
        }

        public static bool operator ==(AppointmentWindow left, AppointmentWindow right)
        {
            return EqualityComparer<AppointmentWindow>.Default.Equals(left, right);
        }

        public static bool operator !=(AppointmentWindow left, AppointmentWindow right)
        {
            return !(left == right);
        }

        #endregion

        public override string ToString() => $"{Start}-{End}";

        public override bool Equals(object obj)
        {
            return Equals(obj as AppointmentWindow);
        }

        public bool Equals(AppointmentWindow other)
        {
            return other != null &&
                   EqualityComparer<TimeOfDay>.Default.Equals(Start, other.Start) &&
                   EqualityComparer<TimeOfDay>.Default.Equals(End, other.End);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Start, End);
        }
    }

    public static class AppointmentWindowExtensions
    {
        public static IAppointmentWindow StartingAt(this AppointmentWindow source, TimeOfDay start) => new AppointmentWindow(start, source.End);
        public static IAppointmentWindow EndingAt(this AppointmentWindow source, TimeOfDay end) => new AppointmentWindow(source.Start, end);
    }
}
