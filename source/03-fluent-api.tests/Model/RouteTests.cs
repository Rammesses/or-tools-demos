using System.Linq;
using _03_fluent_api.Model;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _03_fluent_api.tests.Model
{
    [TestClass]
    public class RouteTests
    {
        [TestMethod]
        public void NewRouteIsEmpty()
        {
            var route = new Route(new Vehicle("Dummy"));
            route.Any().Should().BeFalse();
        }

        [TestMethod]
        public void SettingStartLocationDoesNotAddAnAppointment()
        {
            var route = new Route(new Vehicle("Dummy"));
            route.SetStartLocation(new Location("AB1 1AB"));

            route.Appointments.Should().BeEmpty();
        }

        [TestMethod]
        public void SettingEndLocationDoesNotAddAnAppointment()
        {
            var route = new Route(new Vehicle("Dummy"));
            route.SetEndLocation(new Location("AB1 1AB"));

            route.Appointments.Should().BeEmpty();
        }

        [TestMethod]
        public void SettingStartLocationAddsStartAndEndNodes()
        {
            var route = new Route(new Vehicle("Dummy"));
            route.SetStartLocation(new Location("AB1 1AB"));

            route.Nodes.Count().Should().Be(2);
            route.Nodes.First().Location.Postcode.Should().Be(route.Nodes.Last().Location.Postcode);
        }
    }
}
