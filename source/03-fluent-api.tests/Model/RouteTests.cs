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

        [TestMethod]
        public void SettingEndLocationAddsStartAndEndNodes()
        {
            var route = new Route(new Vehicle("Dummy"));
            route.SetEndLocation(new Location("AB1 1AB"));

            route.Nodes.Count().Should().Be(2);
            route.Nodes.First().Location.Postcode.Should().Be(route.Nodes.Last().Location.Postcode);
        }

        [TestMethod]
        public void SettingEndLocationDoesNotModifyExistingStartNode()
        {
            var route = new Route(new Vehicle("Dummy"));
            route.SetStartLocation(new Location("AB1 1AB"));
            route.SetEndLocation(new Location("CD2 2CD"));

            route.Nodes.Count().Should().Be(2);
            route.Nodes.First().Location.Postcode.Should().Be("AB1 1AB");
            route.Nodes.Last().Location.Postcode.Should().Be("CD2 2CD");
        }

        [TestMethod]
        public void SettingStartLocationDoesNotModifyExistingEndNode()
        {
            var route = new Route(new Vehicle("Dummy"));
            route.SetEndLocation(new Location("CD2 2CD"));
            route.SetStartLocation(new Location("AB1 1AB"));

            route.Nodes.Count().Should().Be(2);
            route.Nodes.First().Location.Postcode.Should().Be("AB1 1AB");
            route.Nodes.Last().Location.Postcode.Should().Be("CD2 2CD");
        }

        [TestMethod]
        public void AddInsertsNodeBetweenExistingStartAndEndNodes()
        {
            var route = new Route(new Vehicle("Dummy"));
            route.SetStartLocation(new Location("AB1 1AB"));
            route.SetEndLocation(new Location("CD2 2CD"));

            route.Add(new Location("EF3 3EF"));

            route.Nodes.Count().Should().Be(3);
            route.Nodes[0].Location.Postcode.Should().Be("AB1 1AB");
            route.Nodes[1].Location.Postcode.Should().Be("EF3 3EF");
            route.Nodes[2].Location.Postcode.Should().Be("CD2 2CD");
        }

        [TestMethod]
        public void AddInsertsNodeBeforeExistingEndNode()
        {
            var route = new Route(new Vehicle("Dummy"));
            route.SetStartLocation(new Location("AB1 1AB"));
            route.SetEndLocation(new Location("CD2 2CD"));

            route.Add(new Location("EF3 3EF"));
            route.Add(new Location("GH4 4GH"));

            route.Nodes.Count().Should().Be(4);
            route.Nodes[0].Location.Postcode.Should().Be("AB1 1AB");
            route.Nodes[1].Location.Postcode.Should().Be("EF3 3EF");
            route.Nodes[2].Location.Postcode.Should().Be("GH4 4GH");
            route.Nodes[3].Location.Postcode.Should().Be("CD2 2CD");
        }
    }
}
