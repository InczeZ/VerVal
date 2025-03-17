namespace DatesAndStuff.Tests
{
    public sealed class SimulationTimeTests
    {
        [OneTimeSetUp]
        public void OneTimeSetupStuff()
        {
            // 
        }

        [SetUp]
        public void Setup()
        {
            // minden teszt felteheti, hogz elotte lefutott ez
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
            throw new NotImplementedException();
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
            DateTime baseDate = new DateTime(2010, 8, 23, 9, 4, 49);
            SimulationTime sut1 = new SimulationTime(baseDate);
            SimulationTime sut2 = new SimulationTime(baseDate);

            bool result = sut1 == sut2;

            Assert.IsTrue(result);

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
                Assert.AreEqual(expectedDateTime, result.ToAbsoluteDateTime());
            }

            [Test]
            //Method_Should_Then
            public void Subtraction_Should_ShiftSimulationTime()
            {
                // code kozelibb
                // RegisterOrder_SignedInUserSendsOrder_OrderIsRegistered
                throw new NotImplementedException();
            }
        }


        public class DifferenceTests
        {

            [Test]
            // simulation difference timespane and datetimetimespan is the same
            public void TwoSimulationTimes_Subtracted_ResultMatchesDateTimeDifference()
            {
                throw new NotImplementedException();
            }
        }

        public class PrecisionTests
        {

            [Test]
            // millisecond representation works
            public void SimulationTime_NextMillisec_IncrementsByOneMillisecond()
            {
                //var t1 = SimulationTime.MinValue.AddMilliseconds(10);
                throw new NotImplementedException();
            }

            [Test]
            // next millisec calculation works
            public void NextMillisec_ShouldCompareCorrectly_ThenIncrementByOneMillisecond()
            {
                //Assert.AreEqual(t1.TotalMilliseconds + 1, t1.NextMillisec.TotalMilliseconds);
                throw new NotImplementedException();
            }
        }

        public class EqualityTests
        {

            [Test]
            // creat a SimulationTime from a DateTime, add the same milliseconds to both and check if they are still equal
            public void SimulationTimeAndDateTime_AfterAddingMilliseconds_RemainEqual()
            {
                throw new NotImplementedException();
            }

            [Test]
            // the same as before just with seconds
            public void SimulationTimeAndDateTime_AfterAddingSeconds_RemainEqual()
            {
                throw new NotImplementedException();
            }

            [Test]
            // same as before just with timespan
            public void SimulationTimeAndDateTime_AfterAddingTimeSpan_RemainEqual()
            {
                throw new NotImplementedException();
            }
        }

        public class StringRepresentationTests
        {
            [Test]
            // check string representation given by ToString
            public void SimulationTime_ToString_ReturnsCorrectStringRepresentation()
            {
                throw new NotImplementedException();
            }
        }
    }
}