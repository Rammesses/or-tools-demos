using System;
namespace _03_fluent_api.Model
{
    public class TimeOfDay : IComparable<TimeOfDay>
    {
        private readonly TimeSpan internalValue;
        public static readonly long DayInSeconds = Convert.ToInt64(
            TimeSpan.FromDays(1).TotalSeconds);

        private static TimeSpan MinTimeSpanValue = new TimeSpan(00, 00, 00);
        private static TimeSpan MaxTimeSpanValue = new TimeSpan(23, 59, 29);

        public static TimeOfDay MinValue = new TimeOfDay(MinTimeSpanValue);
        public static TimeOfDay MaxValue = new TimeOfDay(MaxTimeSpanValue);

        public static TimeOfDay At(string time) => new TimeOfDay(time);

        public TimeOfDay()
        {
            this.internalValue = MinTimeSpanValue;
        }

        public TimeOfDay(short hours, short minutes, short seconds)
            : this (new TimeSpan(hours, minutes, seconds))
        {
        }

        public TimeOfDay(TimeSpan timeSpan)
        {
            var timeToTheSecond = new TimeSpan(timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            if (timeToTheSecond < MinTimeSpanValue || timeToTheSecond > MaxTimeSpanValue)
            {
                throw new ArgumentOutOfRangeException($"{timeSpan} must be between {MinTimeSpanValue} & {MaxTimeSpanValue}");
            }

            this.internalValue = timeToTheSecond;
        }

        public TimeOfDay(string time) : this(TimeSpan.Parse(time))
        {
        }

        public TimeOfDay(TimeOfDay time)
        {
            this.internalValue = time.AsTimeSpan();
        }

        public short Hours => (short)this.internalValue.Hours;
        public short Minutes => (short)this.internalValue.Minutes;
        public short Seconds => (short)this.internalValue.Seconds;

        public override string ToString() => $"{internalValue:g}";

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

    public static class TimeOfDayExtensions
    {
        public static TimeSpan AsTimeSpan(this TimeOfDay time)
        {
            return new TimeSpan(time.Hours, time.Minutes, time.Seconds);
        }
    }
}
