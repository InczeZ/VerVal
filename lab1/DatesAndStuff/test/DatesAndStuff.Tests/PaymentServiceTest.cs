using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DatesAndStuff.Tests
{
    internal class PaymentServiceTest
    {
        [Test]
        public void TestPaymentService_ManualMock_SufficientBalance()
        {
            // Arrange
            var testPaymentService = new TestPaymentService(Person.SubscriptionFee + 10);
            Person sut = new Person("Test Pista",
                new EmploymentInformation(
                    54,
                    new Employer("RO1234567", "Valami city valami hely", "Dagobert bacsi", new List<int>() { 6201, 7210 })
                ),
                testPaymentService,
                new LocalTaxData("4367558"),
                new FoodPreferenceParams()
                {
                    CanEatChocolate = true,
                    CanEatEgg = true,
                    CanEatLactose = true,
                    CanEatGluten = true
                }
            );

            // Act
            bool result = sut.PerformSubscriptionPayment();

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void TestPaymentService_ManualMock_InsufficientBalance()
        {
            var testPaymentService = new TestPaymentService(Person.SubscriptionFee + 10);
            Person sut = new Person("Test Pista",
                new EmploymentInformation(
                    54,
                    new Employer("RO1234567", "Valami city valami hely", "Dagobert bacsi", new List<int>() { 6201, 7210 })
                ),
                testPaymentService,
                new LocalTaxData("4367558"),
                new FoodPreferenceParams()
                {
                    CanEatChocolate = true,
                    CanEatEgg = true,
                    CanEatLactose = true,
                    CanEatGluten = true
                }
            );

            // Act
            bool result = sut.PerformSubscriptionPayment();

            // Assert
            result.Should().BeTrue();
            testPaymentService.Balance.Should().Be(Person.SubscriptionFee + 10 - Person.SubscriptionFee);
        }

        [Test]
        public void TestPaymentService_Mock_SufficientBalance()
        {
            // Arrange
            var paymentSequence = new MockSequence();
            var paymentService = new Mock<IPaymentService>();

            paymentService.InSequence(paymentSequence).Setup(m => m.StartPayment());
            paymentService.InSequence(paymentSequence).Setup(m => m.Balance).Returns(Person.SubscriptionFee + 10);
            paymentService.InSequence(paymentSequence).Setup(m => m.SpecifyAmount(Person.SubscriptionFee));
            paymentService.InSequence(paymentSequence).Setup(m => m.ConfirmPayment());
            paymentService.InSequence(paymentSequence).Setup(m => m.SuccessFul()).Returns(true);

            var paymentServiceMock = paymentService.Object;

            Person sut = new Person("Test Pista",
                new EmploymentInformation(
                    54,
                    new Employer("RO1234567", "Valami city valami hely", "Dagobert bacsi", new List<int>() { 6201, 7210 })
                ),
                paymentServiceMock,
                new LocalTaxData("4367558"),
                new FoodPreferenceParams()
                {
                    CanEatChocolate = true,
                    CanEatEgg = true,
                    CanEatLactose = true,
                    CanEatGluten = true
                }
            );

            // Act
            bool result = sut.PerformSubscriptionPayment();

            // Assert
            result.Should().BeTrue();
            paymentService.Verify(m => m.StartPayment(), Times.Once);
            paymentService.Verify(m => m.Balance, Times.Once);
            paymentService.Verify(m => m.SpecifyAmount(Person.SubscriptionFee), Times.Once);
            paymentService.Verify(m => m.ConfirmPayment(), Times.Once);
            paymentService.Verify(m => m.SuccessFul(), Times.Once);
        }

        [Test]
        public void TestPaymentService_Mock_InsufficientBalance()
        {
            // Arrange
            var paymentSequence = new MockSequence();
            var paymentService = new Mock<IPaymentService>();

            paymentService.InSequence(paymentSequence).Setup(m => m.StartPayment());
            paymentService.InSequence(paymentSequence).Setup(m => m.Balance).Returns(Person.SubscriptionFee - 10); // Insufficient balance
            paymentService.InSequence(paymentSequence).Setup(m => m.SpecifyAmount(Person.SubscriptionFee));
            paymentService.InSequence(paymentSequence).Setup(m => m.CancelPayment());
            paymentService.InSequence(paymentSequence).Setup(m => m.SuccessFul()).Returns(false);

            var paymentServiceMock = paymentService.Object;

            Person sut = new Person("Test Pista",
                new EmploymentInformation(
                    54,
                    new Employer("RO1234567", "Valami city valami hely", "Dagobert bacsi", new List<int>() { 6201, 7210 })
                ),
                paymentServiceMock,
                new LocalTaxData("4367558"),
                new FoodPreferenceParams()
                {
                    CanEatChocolate = true,
                    CanEatEgg = true,
                    CanEatLactose = true,
                    CanEatGluten = true
                }
            );

            // Act
            Action act = () => sut.PerformSubscriptionPayment();

            // Assert
            act.Should().Throw<InvalidOperationException>();
            paymentService.Verify(m => m.CancelPayment(), Times.Once);
            paymentService.Verify(m => m.Balance, Times.Once);
        }


    }
}
