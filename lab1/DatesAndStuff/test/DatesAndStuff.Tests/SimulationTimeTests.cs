using FluentAssertions;

namespace DatesAndStuff.Tests
{
    public sealed class SimulationTimeTests
    {
        DateTime baseDate;
        SimulationTime sut;

        [SetUp]
        public void Setup()
        {
            this.baseDate = new DateTime(2010, 8, 23, 9, 4, 49);
            this.sut = new SimulationTime(baseDate);
        }

        [OneTimeSetUp]
        public void OneTimeSetupStuff()
        {
            // 
        }

        [TearDown]
        public void TearDown()
        {
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
        }

        [Test]
        // Default time is not current time.
        public void SimulationTime_Construction_DefaultTimeIsCurrentTime()
        {
            // throw new NotImplementedException();
        }

        [Test]
        // equal
        // not equal
        // <
        // >
        // <= different
        // >= different 
        // <= same
        // >= same
        // max
        // min
        public void TwoSimulationTimes_Compared_ComparisonWorksCorrectly()
        {
            // Arrange
            SimulationTime simulationTime = new SimulationTime(this.baseDate);

            // Act
            Boolean result = this.sut == simulationTime;

            // Assert
            result.Should().BeTrue();

        }

        private class TimeSpanArithmeticTests
        {

            [Test]
            // TimeSpanArithmetic
            // add
            // substract
            // Given_When_Then
            public void SimulationTime_TimeSpanAdded_TimeIsShifted()
            {
                // UserSignedIn_OrderSent_OrderIsRegistered
                // DBB, specflow, cucumber, gherkin

                // Arrange
                DateTime baseDate = new DateTime(2010, 8, 23, 9, 4, 49);
                SimulationTime sut = new SimulationTime(baseDate);

                var ts = TimeSpan.FromMilliseconds(4544313);

                // Act
                var result = sut + ts;

                // Assert
                var expectedDateTime = baseDate + ts;
                expectedDateTime.Should().Be(result.ToAbsoluteDateTime());
            }

            [Test]
            //Method_Should_Then
            public void Subtraction_Should_ShiftSimulationTime()
            {
                // code kozelibb
                // RegisterOrder_SignedInUserSendsOrder_OrderIsRegistered
                // throw new NotImplementedException();
            }
        }


        public class DifferenceTests
        {
            private SimulationTime sut;
            private DateTime baseDate;

            [SetUp]
            public void Setup()
            {
                this.baseDate = new DateTime(2010, 8, 23, 9, 4, 49);
                this.sut = new SimulationTime(baseDate);
            }

            [Test]
            // simulation difference timespane and datetimetimespan is the same
            public void TwoSimulationTimes_Subtracted_ResultMatchesDateTimeDifference()
            {
                // Arrange
                DateTime baseDate = new DateTime(2023, 10, 1, 12, 5, 0);
                SimulationTime simulationTime = new SimulationTime(baseDate);

                // Act
                TimeSpan simulationTimeDifference = simulationTime - this.sut;
                TimeSpan dateTimeDifference = baseDate - this.baseDate;

                // Assert
                dateTimeDifference.Should().Be(simulationTimeDifference);
            }
        }

        public class PrecisionTests
        {

            [Test]
            // millisecond representation works
            public void SimulationTime_NextMillisec_IncrementsByOneMillisecond()
            {
                //var t1 = SimulationTime.MinValue.AddMilliseconds(10);
                // throw new NotImplementedException();
            }

            [Test]
            // next millisec calculation works
            public void NextMillisec_ShouldCompareCorrectly_ThenIncrementByOneMillisecond()
            {
                //Assert.AreEqual(t1.TotalMilliseconds + 1, t1.NextMillisec.TotalMilliseconds);
                // throw new NotImplementedException();
            }
        }

        public class EqualityTests
        {
            private SimulationTime sut;
            private DateTime baseDate;

            [SetUp]
            public void Setup()
            {
                this.baseDate = new DateTime(2010, 8, 23, 9, 4, 49);
                this.sut = new SimulationTime(baseDate);
            }

            [Test]
            // creat a SimulationTime from a DateTime, add the same milliseconds to both and check if they are still equal
            public void SimulationTimeAndDateTime_AfterAddingMilliseconds_RemainEqual()
            {
                // Arrange
                DateTime dateTime = this.baseDate.AddMilliseconds(1);
                SimulationTime simulationTime = new SimulationTime(dateTime);

                // Act
                this.sut = this.sut.AddMilliseconds(1);

                // Assert
                this.sut.Should().Be(simulationTime);

            }

            [Test]
            // the same as before just with seconds
            public void SimulationTimeAndDateTime_AfterAddingSeconds_RemainEqual()
            {
                // Arrange
                DateTime dateTime = this.baseDate.AddSeconds(1);
                SimulationTime simulationTime = new SimulationTime(dateTime);

                // Act
                this.sut = this.sut.AddSeconds(1);

                // Assert
                this.sut.Should().Be(simulationTime);
            }

            [Test]
            // same as before just with timespan
            public void SimulationTimeAndDateTime_AfterAddingTimeSpan_RemainEqual()
            {
                // Arrange
                SimulationTime simulationTime = new SimulationTime(baseDate);
                TimeSpan timeSpan = TimeSpan.FromSeconds(420);

                // Act
                this.sut.AddTimeSpan(timeSpan);
                simulationTime.AddTimeSpan(timeSpan);

                // Assert
                this.sut.Should().Be(simulationTime);
            }
        }

        public class StringRepresentationTests
        {
            [Test]
            // check string representation given by ToString
            public void SimulationTime_ToString_ReturnsCorrectStringRepresentation()
            {
                // throw new NotImplementedException();
            }
        }
    }
}