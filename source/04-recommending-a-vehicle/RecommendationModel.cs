using System;

using _03_fluent_api;
using _03_fluent_api.Model;

namespace _04_recommending_a_vehicle
{
    public class RecommendationModel : ProblemModel
    {
        private RecommendationModel()
        {
        }

        public static ProblemModel Create()
        {
            var problem = new RecommendationModel();

            problem.AddVehicleRoute("Bob")
                .LeavingFrom("RG1 1AA")
                .WithShift(Shift.StartingAt(TimeOfDay.At("09:00:00"))
                                .EndingAt(TimeOfDay.At("17:00:00")))
                .WithAppointment(Appointment.At("RG2 2BB")
                                            .Between(TimeOfDay.At("09:00:00"), TimeOfDay.At("12:00:00")))
                .WithAppointment(Appointment.At("RG3 3CC")
                                            .Between(TimeOfDay.At("09:00:00"), TimeOfDay.At("12:00:00")))
                .ReturningToStart();

            problem.AddVehicleRoute("Eric")
                .LeavingFrom("RG1 1AA")
                .WithShift(Shift.StartingAt(TimeOfDay.At("10:00:00"))
                                .EndingAt(TimeOfDay.At("15:30:00")))
                .WithAppointment(Appointment.At("RG2 2BB")
                                            .Between(TimeOfDay.At("09:00:00"), TimeOfDay.At("17:00:00")))
                .WithAppointment(Appointment.At("RG3 3CC")
                                            .Between(TimeOfDay.At("09:00:00"), TimeOfDay.At("17:00:00")))
                .ReturningTo("RG9 9II");

            return problem;
        }
    }
}
