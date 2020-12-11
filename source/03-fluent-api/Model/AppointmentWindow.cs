using System;
namespace _03_fluent_api.Model
{
    public interface IAppointmentWindow
    {
        TimeOfDay Start { get; }
        TimeOfDay End { get; }
    }

    public class AppointmentWindow : IAppointmentWindow
    {
        public static IAppointmentWindow Default = new AppointmentWindow(TimeOfDay.At("07:00:00"), TimeOfDay.At("22:00:00"));
        public static IAppointmentWindow StartingAt(TimeOfDay start) => new AppointmentWindow(start, Default.End);
        public static IAppointmentWindow EndingAt(TimeOfDay end) => new AppointmentWindow(Default.Start, end);

        public AppointmentWindow()
        {
            this.Start = TimeOfDay.MinValue;
            this.End = TimeOfDay.MaxValue;
        }

        public AppointmentWindow(TimeOfDay start, TimeOfDay end)
        {
            this.Start = start;
            this.End = end;
        }

        public TimeOfDay Start { get; }
        public TimeOfDay End { get; }

        public override string ToString() => $"{Start}-{End}";
    }

    public static class AppointmentWindowExtensions
    {
        public static IAppointmentWindow StartingAt(this AppointmentWindow source, TimeOfDay start) => new AppointmentWindow(start, source.End);
        public static IAppointmentWindow EndingAt(this AppointmentWindow source, TimeOfDay end) => new AppointmentWindow(source.Start, end);
    }
}
