using System;
using _03_fluent_api.Model;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _03_fluent_api.tests.Model
{
    [TestClass]
    public class ShiftTests
    {
        [TestMethod]
        public void DefaultCtorReturnsAllDayShift()
        {
            var allDayShift = new Shift();
            allDayShift.Start.Should().Be(TimeOfDay.MinValue);
            allDayShift.End.Should().Be(TimeOfDay.MaxValue);
        }

        [TestMethod]
        public void RangeCtorReturnsShiftCorrectly()
        {
            var start = TimeOfDay.At("09:00:00");
            var end = TimeOfDay.At("17:30:00");

            var shift = new Shift(start, end);
            shift.Start.Should().Be(TimeOfDay.At("09:00:00"));
            shift.End.Should().Be(TimeOfDay.At("17:30:00"));
        }

        [TestMethod]
        public void RangeCtorThrowsArgumentOutOfRangeForReversedStartAndEnd()
        {
            var start = TimeOfDay.At("09:00:00");
            var end = TimeOfDay.At("17:30:00");

            Action act = () => new Shift(end, start);
            act.Should().Throw<ArgumentOutOfRangeException>().Where(e => e.ParamName == "end");
        }

        [TestMethod]
        public void StartingAtReturnsShiftWithStartSetAndEndDefault()
        {
            var shift = Shift.StartingAt(TimeOfDay.At("09:00:00"));
            shift.Start.Should().Be(TimeOfDay.At("09:00:00"));
            shift.End.Should().Be(TimeOfDay.MaxValue);
        }

        [TestMethod]
        public void EndingAtReturnsShiftWithEndSetAndStartDefault()
        {
            var shift = Shift.EndingAt(TimeOfDay.At("17:00:00"));
            shift.Start.Should().Be(TimeOfDay.MinValue);
            shift.End.Should().Be(TimeOfDay.At("17:00:00"));
        }

        [TestMethod]
        public void StartingAtReturnsNewShiftAndChangesExistingStart()
        {
            var shift = new Shift(TimeOfDay.At("09:00:00"), TimeOfDay.At("17:00:00"));
            var modified = shift.StartingAt(TimeOfDay.At("05:00:00"));

            modified.Should().NotBeSameAs(shift);
            modified.Start.Should().Be(TimeOfDay.At("05:00:00"));
            modified.End.Should().Be(TimeOfDay.At("17:00:00"));
        }

        [TestMethod]
        public void EndingAtReturnsNewShiftAndChangesExistingEnd()
        {
            var shift = new Shift(TimeOfDay.At("09:00:00"), TimeOfDay.At("17:00:00"));
            var modified = shift.EndingAt(TimeOfDay.At("14:00:00"));

            modified.Should().NotBeSameAs(shift);
            modified.Start.Should().Be(TimeOfDay.At("09:00:00"));
            modified.End.Should().Be(TimeOfDay.At("14:00:00"));
        }

    }
}
