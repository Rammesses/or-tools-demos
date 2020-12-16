using System;
using _03_fluent_api.Model;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _03_fluent_api.tests.Model
{
    [TestClass]
    public class AppointmentWindowTests
    {
        [TestMethod]
        public void DefaultCtorReturnsNewDefaultWindow() 
        {
            var window = new AppointmentWindow();

            window.Should().NotBeSameAs((AppointmentWindow)AppointmentWindow.Default);
            window.Start.Should().Be(AppointmentWindow.Default.Start);
            window.End.Should().Be(AppointmentWindow.Default.End);
        }

        [TestMethod]
        public void SourceCtorReturnsNewWindowWithValuesFromSource()
        {
            var source = new AppointmentWindow(TimeOfDay.At("09:00:00"), TimeOfDay.At("17:00:00"));
            var window = new AppointmentWindow(source);

            window.Should().NotBeSameAs(source);
            window.Start.Should().Be(source.Start);
            window.End.Should().Be(source.End);
        }

        [TestMethod]
        public void RangeCtorReturnsWindowCorrectly()
        {
            var start = TimeOfDay.At("09:00:00");
            var end = TimeOfDay.At("17:30:00");

            var Window = new AppointmentWindow(start, end);
            Window.Start.Should().Be(TimeOfDay.At("09:00:00"));
            Window.End.Should().Be(TimeOfDay.At("17:30:00"));
        }

        [TestMethod]
        public void RangeCtorThrowsArgumentOutOfRangeForReversedStartAndEnd()
        {
            var start = TimeOfDay.At("09:00:00");
            var end = TimeOfDay.At("17:30:00");

            Action act = () => new AppointmentWindow(end, start);
            act.Should().Throw<ArgumentOutOfRangeException>().Where(e => e.ParamName == "end");
        }

        [TestMethod]
        public void StartingAtReturnsWindowWithStartSetAndEndDefault()
        {
            var window = AppointmentWindow.StartingAt(TimeOfDay.At("09:00:00"));
            window.Start.Should().Be(TimeOfDay.At("09:00:00"));
            window.End.Should().Be(AppointmentWindow.Default.End);
        }

        [TestMethod]
        public void EndingAtReturnsWindowWithEndSetAndStartDefault()
        {
            var window = AppointmentWindow.EndingAt(TimeOfDay.At("17:00:00"));
            window.Start.Should().Be(AppointmentWindow.Default.Start);
            window.End.Should().Be(TimeOfDay.At("17:00:00"));
        }

        [TestMethod]
        public void StartingAtReturnsNewWindowAndChangesExistingStart()
        {
            var window = new AppointmentWindow(TimeOfDay.At("09:00:00"), TimeOfDay.At("17:00:00"));
            var modified = window.StartingAt(TimeOfDay.At("05:00:00"));

            modified.Should().NotBeSameAs(window);
            modified.Start.Should().Be(TimeOfDay.At("05:00:00"));
            modified.End.Should().Be(TimeOfDay.At("17:00:00"));
        }

        [TestMethod]
        public void EndingAtReturnsNewWindowAndChangesExistingEnd()
        {
            var window = new AppointmentWindow(TimeOfDay.At("09:00:00"), TimeOfDay.At("17:00:00"));
            var modified = window.EndingAt(TimeOfDay.At("14:00:00"));

            modified.Should().NotBeSameAs(window);
            modified.Start.Should().Be(TimeOfDay.At("09:00:00"));
            modified.End.Should().Be(TimeOfDay.At("14:00:00"));
        }


        [TestMethod]
        public void ComparisonOperatorsWorkForNonOverlappingWindows()
        {
            var window1 = new AppointmentWindow(TimeOfDay.At("09:00:00"), TimeOfDay.At("12:00:00"));
            var window2 = new AppointmentWindow(TimeOfDay.At("13:00:00"), TimeOfDay.At("17:00:00"));

            (window1 > window2).Should().BeFalse();
            (window1 < window2).Should().BeTrue();
            (window1 == window2).Should().BeFalse();
            (window1 >= window2).Should().BeFalse();
            (window1 <= window2).Should().BeTrue();
            (window1 != window2).Should().BeTrue();

            var window1Clone = new AppointmentWindow(window1);
            (window1Clone == window1).Should().BeTrue();
            (window1Clone != window1).Should().BeFalse();
            (window1Clone >= window1).Should().BeTrue();
            (window1Clone <= window1).Should().BeTrue();
        }

        [TestMethod]
        public void ComparisonOperatorsWorkForOverlappingWindows()
        {
            var window1 = new AppointmentWindow(TimeOfDay.At("09:00:00"), TimeOfDay.At("14:00:00"));
            var window2 = new AppointmentWindow(TimeOfDay.At("13:00:00"), TimeOfDay.At("17:00:00"));

            (window1 > window2).Should().BeFalse();
            (window1 < window2).Should().BeTrue();
            (window1 == window2).Should().BeFalse();
            (window1 >= window2).Should().BeFalse();
            (window1 <= window2).Should().BeTrue();
            (window1 != window2).Should().BeTrue();
        }

        [TestMethod]
        public void ComparisonOperatorsWorkForSmallerWithinLargerWindows()
        {
            var window1 = new AppointmentWindow(TimeOfDay.At("09:00:00"), TimeOfDay.At("17:00:00"));
            var window2 = new AppointmentWindow(TimeOfDay.At("13:00:00"), TimeOfDay.At("14:00:00"));

            (window1 == window2).Should().BeFalse();

            // window1 surrounds window 2
            (window1 > window2).Should().BeTrue();
            (window1 < window2).Should().BeFalse();
            (window1 >= window2).Should().BeTrue();
            (window1 <= window2).Should().BeFalse();
            (window1 != window2).Should().BeTrue();
        }

    }
}
