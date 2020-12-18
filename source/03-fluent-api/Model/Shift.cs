using System;

namespace _03_fluent_api.Model
{
    public interface IShift
    {
        TimeOfDay Start { get; }
        TimeOfDay End { get; }
    }

    public class Shift : IShift
    {
        public static Shift StartingAt(TimeOfDay start) => new Shift(start, TimeOfDay.MaxValue);
        public static Shift EndingAt(TimeOfDay end) => new Shift(TimeOfDay.MinValue, end);

        public Shift()
        {
            this.Start = TimeOfDay.MinValue;
            this.End = TimeOfDay.MaxValue;
        }

        public Shift(TimeOfDay start, TimeOfDay end)
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

        public override string ToString()
        {
            return $"{Start}-{End}";
        }
    }

    public static class ShiftExtensions
    {
        public static Shift StartingAt(this Shift source, TimeOfDay start) => new Shift(start, source.End);
        public static Shift EndingAt(this Shift source, TimeOfDay end) => new Shift(source.Start, end);
    }
}