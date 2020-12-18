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

        public static RecommendationModel Create()
        {
            var problem = new RecommendationModel();

            var bobsShift = Shift.StartingAt(TimeOfDay.At("09:00:00"))
                                .EndingAt(TimeOfDay.At("17:00:00"));

            var bobsRoute = problem.AddVehicleRoute("Bob")
                                   .WithShift(bobsShift)
                                   .LeavingFrom("RG1 1AA")
                                   .ReturningToStart();

            var appointment1 =
                Appointment.At("RG2 2BB")
                           .Between(TimeOfDay.At("09:00:00"),
                                    TimeOfDay.At("12:00:00"));
            bobsRoute.WithAppointment(appointment1);

            var appointment2 =
                Appointment.At("RG3 3CC")
                           .Between(TimeOfDay.At("09:00:00"),
                                    TimeOfDay.At("12:00:00"));
            bobsRoute.WithAppointment(appointment2);

            problem.AddVehicleRoute("Eric")
                .WithShift(Shift.StartingAt(TimeOfDay.At("10:00:00"))
                                .EndingAt(TimeOfDay.At("15:30:00")))
                .LeavingFrom("RG1 1AA")
                .WithAppointment(Appointment.At("RG2 2BB")
                                            .Between(TimeOfDay.At("09:00:00"), TimeOfDay.At("17:00:00")))
                .WithAppointment(Appointment.At("RG3 3CC")
                                            .Between(TimeOfDay.At("09:00:00"), TimeOfDay.At("17:00:00")))
                .ReturningTo("RG9 9II");

            problem.AddVehicleRoute("Frank")
                .WithShift(Shift.StartingAt(TimeOfDay.At("06:00:00"))
                                .EndingAt(TimeOfDay.At("12:30:00")))
                .LeavingFrom("RG1 1AA")
                .WithAppointment(Appointment.At("RG3 3CC")
                                            .Between(TimeOfDay.At("09:00:00"), TimeOfDay.At("17:00:00")))
                .WithAppointment(Appointment.At("RG8 8BB")
                                            .Between(TimeOfDay.At("09:00:00"), TimeOfDay.At("17:00:00")))
                .WithAppointment(Appointment.At("RG3 3CC")
                                            .Between(TimeOfDay.At("09:00:00"), TimeOfDay.At("17:00:00")))
                .ReturningToStart();

            return problem;
        }
    }
}
