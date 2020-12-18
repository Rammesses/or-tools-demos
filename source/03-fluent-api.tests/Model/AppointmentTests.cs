using System;
using _03_fluent_api.Model;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _03_fluent_api.tests.Model
{
    [TestClass]
    public class AppointmentTests
    {
        public void AppointmentsAreEqualIfAtSameLocationWithSameWindow()
        {
            var appt1 = new Appointment(
                new Location("AB1 1AB"),
                new AppointmentWindow(TimeOfDay.At("09:00:00"), TimeOfDay.At("10:00:00")));
            var appt2 = new Appointment(
                new Location("AB1 1AB"),
                new AppointmentWindow(TimeOfDay.At("09:00:00"), TimeOfDay.At("10:00:00")));

            (appt1 == appt2).Should().BeTrue();
        }
    }
}
