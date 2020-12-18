using _03_fluent_api.Model;
using _04_recommending_a_vehicle.Model;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _04_recommending_a_vehicle.tests.Model
{
    [TestClass]
    public class SolutionRouteTests
    {
        [TestMethod]
        public void CtorCopiesSolutionRoute()
        {
            var route = new Route(new Vehicle("Bob"));
            route.SetStartLocation(new Location("AB1 1AB"));
            route.SetEndLocation(new Location("CD2 2CD"));
            route.Add(new Location("EF3 3EF"));
            route.Add(new Location("GH4 4GH"));
            route.Add(new Location("IJ5 5IJ"));

            var solutionRoute = new SolutionRoute(route, 0);

            solutionRoute.SolvedRoute.Should().NotBeSameAs(route);
            solutionRoute.SolvedRoute.StartLocation.Should().Be(route.StartLocation);
            solutionRoute.SolvedRoute.EndLocation.Should().Be(route.EndLocation);
            solutionRoute.Nodes.Should().BeEquivalentTo(route.Nodes);
        }
    }
}
