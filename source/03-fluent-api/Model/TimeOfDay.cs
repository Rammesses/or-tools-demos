using System;
namespace _03_fluent_api.Model
{
    public class TimeOfDay : IComparable<TimeOfDay>
    {
        private readonly TimeSpan internalValue;
        public static readonly long DayInSeconds = Convert.ToInt64(
            TimeSpan.FromDays(1).TotalSeconds);

        public static TimeOfDay MinValue = new TimeOfDay("00:00:00");
        public static TimeOfDay MaxValue = new TimeOfDay("23:59:59");

        public static TimeOfDay At(string time) => new TimeOfDay(time);

        public TimeOfDay(string time)
        {
            this.internalValue = TimeSpan.Parse(time);
        }

        public int CompareTo(TimeOfDay other)
        {
            if (other == null) return 1;
            return internalValue.CompareTo(other.internalValue);
        }

        public static bool operator >(TimeOfDay operand1, TimeOfDay operand2)
        {
            return operand1.CompareTo(operand2) == 1;
        }

        public static bool operator <(TimeOfDay operand1, TimeOfDay operand2)
        {
            return operand1.CompareTo(operand2) == -1;
        }

        public static bool operator >=(TimeOfDay operand1, TimeOfDay operand2)
        {
            return operand1.CompareTo(operand2) >= 0;
        }

        public static bool operator <=(TimeOfDay operand1, TimeOfDay operand2)
        {
            return operand1.CompareTo(operand2) <= 0;
        }
    }
}
