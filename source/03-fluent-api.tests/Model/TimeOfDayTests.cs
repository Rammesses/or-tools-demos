using System;
using _03_fluent_api.Model;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _03_fluent_api.tests.Model
{
    [TestClass]
    public class TimeOfDayTests
    {
        [TestMethod]
        public void DefaultCtorReturnsMidnight()
        {
            var timeOfDay = new TimeOfDay();
            timeOfDay.Should().Be(TimeOfDay.MinValue);            
        }

        [TestMethod]
        public void HmsCtorReturnsTimeOfDayCorrectly()
        {
            var timeOfDay = new TimeOfDay(7, 8, 9);
            timeOfDay.Hours.Should().Be(7);
            timeOfDay.Minutes.Should().Be(8);
            timeOfDay.Seconds.Should().Be(9);
        }

        [TestMethod]
        public void TimespanCtorReturnsTimeOfDayCorrectly()
        {
            var timeOfDay = new TimeOfDay(new TimeSpan(7, 8, 9));
            timeOfDay.Hours.Should().Be(7);
            timeOfDay.Minutes.Should().Be(8);
            timeOfDay.Seconds.Should().Be(9);
        }

        [TestMethod]
        public void StringCtorReturnsTimeOfDayCorrectly()
        {
            var timeOfDay = new TimeOfDay("07:08:09");
            timeOfDay.Hours.Should().Be(7);
            timeOfDay.Minutes.Should().Be(8);
            timeOfDay.Seconds.Should().Be(9);
        }

        [TestMethod]
        public void CtorThrowsArgumentOutOfRangeExceptionForTimeOfDayLessThanMin()
        {
            var negativeTime = new TimeSpan(0, 0, -1);
            Action act = () => new TimeOfDay(negativeTime);
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void OperatorsWorkCorrectly()
        {
            var tenAM = TimeOfDay.At("10:00:00");
            var elevenAM = TimeOfDay.At("11:00:00");

            (tenAM == elevenAM).Should().BeFalse();
            (tenAM < elevenAM).Should().BeTrue();
            (tenAM > elevenAM).Should().BeFalse();
            (tenAM <= tenAM).Should().BeTrue();
            (tenAM <= elevenAM).Should().BeTrue();
            (tenAM >= tenAM).Should().BeTrue();
            (tenAM >= elevenAM).Should().BeFalse();

            (elevenAM < tenAM).Should().BeFalse();
            (elevenAM > tenAM).Should().BeTrue();
            (elevenAM <= tenAM).Should().BeFalse();
            (elevenAM >= tenAM).Should().BeTrue();
        }
    }
}
