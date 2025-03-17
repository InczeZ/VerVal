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
            this.sut = new Person("Test Pista", 54);
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
            string newName = "Test-Eleso-Felallo Pista";
            sut.GotMarried("");

            // Act
            var task = Task.Run(() => sut.GotMarried(""));
            try { task.Wait(); } catch { }

            // Assert
            task.IsFaulted.Should().BeTrue();
        }
    }

    public class SalaryTests
    {
        Person sut;

        [SetUp]
        public void Setup()
        {
            this.sut = new Person("Test Pista", 54);
        }

        [Test]
        public void Salary_PositiveIncrease_ShouldIncrease() // fix 1. exercise
        {
            // Arrange
            double salary = sut.Salary;

            // Act
            this.sut.IncreaseSalary(10);

            // Assert
            salary.Should().BeLessThan(sut.Salary);

        }

        [Test]
        public void Salary_ZeroPercentIncrease_ShouldNotChange()
        {
            // Arrange
            double salary = sut.Salary;

            // Act
            this.sut.IncreaseSalary(0);

            // Assert
            salary.Should().Be(sut.Salary);
        }

        [Test]
        public void Salary_NegativeIncrease_ShouldDecrease()
        {
            // Arrange
            double salary = sut.Salary;

            // Act
            this.sut.IncreaseSalary(-1);

            // Assert
            salary.Should().BeGreaterThan(sut.Salary);
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
