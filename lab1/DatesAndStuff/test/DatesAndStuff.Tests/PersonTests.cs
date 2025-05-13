using AutoFixture;
using FluentAssertions;

namespace DatesAndStuff.Tests;

public class PersonTests
{
    public class MarriageTests
    {
        Person sut;

        [SetUp]
        public void Setup()
        {
            this.sut = PersonFactory.CreateTestPerson();
        }

        [Test]
        public void GotMerried_First_NameShouldChange()
        {
            // Arrange
            string newName = "Test-Eleso Pista";
            double salaryBeforeMarriage = sut.Salary;
            var beforeChanges = Person.Clone(sut);

            // Act
            sut.GotMarried(newName);

            // Assert
            Assert.That(sut.Name, Is.EqualTo(newName)); // act = exp

            sut.Name.Should().Be(newName);
            sut.Should().BeEquivalentTo(beforeChanges, o => o.Excluding(p => p.Name));

            //sut.Salary.Should().Be(salaryBeforeMarriage);

            //Assert.AreEqual(newName, sut.Name); // = (exp, act)
            //Assert.AreEqual(salaryBeforeMarriage, sut.Salary);
        }

        [Test]
        public void GotMerried_Second_ShouldFail()
        {
            // Arrange
            // Arrange
            var fixture = new AutoFixture.Fixture();
            fixture.Customize<IPaymentService>(c => c.FromFactory(() => new TestPaymentService()));

            var sut = fixture.Create<Person>();

            string newName = "Test-Eleso-Felallo Pista";
            sut.GotMarried("");

            // Act
            var task = Task.Run(() => sut.GotMarried(""));
            try { task.Wait(); } catch { }

            // Assert
            Assert.IsTrue(task.IsFaulted);
        }
    }

    [TestFixture]
    public class SalaryTests
    {
        Person sut;

        [SetUp]
        public void Setup()
        {
            this.sut = PersonFactory.CreateTestPerson();
        }

        [Test]
        [TestCase(0.01)]
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(15)]
        [TestCase(20)]
        public void IncreaseSalary_ValidPercentage_ShouldModifySalary(double salaryIncreasePercentage)
        {
            // Arrange
            double initialSalary = sut.Salary;

            // Act
            sut.IncreaseSalary(salaryIncreasePercentage);

            // Assert
            sut.Salary.Should().BeApproximately(initialSalary * (100 + salaryIncreasePercentage) / 100,
                Math.Pow(10, -8),
                because: "numerical salary calculation might be rounded to conform legal stuff");
        }

        [TestCase(-50)]
        [TestCase(-100)]
        public void IncreaseSalary_InvalidPercentage_ShouldNotModifySalary(double salaryIncreasePercentage)
        {
            // Arrange
            double originalSalary = sut.Salary;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => sut.IncreaseSalary(salaryIncreasePercentage));
            sut.Salary.Should().Be(originalSalary, "Invalid salary increases should not modify the salary.");
        }


        [Test]
        public void Constructor_DefaultParams_ShouldBeAbleToEatChocolate()
        {
            // Arrange

            // Act
            Person sut = PersonFactory.CreateTestPerson();

            // Assert
            sut.CanEatChocolate.Should().BeTrue();
        }

        [Test]
        public void Constructor_DontLikeChocolate_ShouldNotBeAbleToEatChocolate()
        {
            // Arrange

            // Act
            Person sut = PersonFactory.CreateTestPerson(fp => fp.CanEatChocolate = false);

            // Assert
            sut.CanEatChocolate.Should().BeFalse();
        }

        [Test]
        public void Salary_PositiveIncrease_ShouldIncrease()
        {
            // Arrange
            double salary = sut.Salary;

            // Act
            this.sut.IncreaseSalary(10);

            // Assert
            salary.Should().BeLessThan(sut.Salary);

        }

        [Test]
        public void Salary_SmallerThanMinusTenPerc_ShouldFail()
        {
            // Arrange
            double salary = sut.Salary;

            // Act
            var task = Task.Run(() => sut.IncreaseSalary(-10.1));
            try { task.Wait(); } catch { }

            // Assert
            task.IsFaulted.Should().BeTrue();
        }
    }
}
