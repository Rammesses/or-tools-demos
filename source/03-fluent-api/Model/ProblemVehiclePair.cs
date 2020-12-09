using System;
namespace _03_fluent_api.Model
{
    public interface IProblemVehiclePair
    {
        IProblemModel Problem { get; }
        IVehicle Vehicle { get; }
    }

    public class ProblemVehiclePair : Tuple<IProblemModel, IVehicle>, IProblemVehiclePair
    {
        public ProblemVehiclePair(IProblemModel item1, IVehicle item2) : base(item1, item2)
        {
        }

        public IProblemModel Problem => base.Item1;

        public IVehicle Vehicle => base.Item2;
    }

    public static class ProblemVehiclePairExtensions
    {
        public static IProblemVehiclePair WithAppointment(this IProblemVehiclePair pair, string location, AppointmentWindow window)
        {
            var appointment = Appointment.At(location).Within(window);
            pair.Problem.Routes.For(pair.Vehicle).Add(appointment);
            return pair;
        }
    }
}
