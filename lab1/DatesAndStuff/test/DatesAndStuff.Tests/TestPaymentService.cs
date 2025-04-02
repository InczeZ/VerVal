using System;

namespace DatesAndStuff.Tests
{
    internal class TestPaymentService : IPaymentService
    {
        private uint startCallCount = 0;
        private uint specifyCallCount = 0;
        private uint confirmCallCount = 0;
        private double v;

        public TestPaymentService(double v)
        {
            this.v = v;
        }

        public TestPaymentService()
        {
        }

        public double Balance { get; set; }

        public void StartPayment()
        {
            if (startCallCount != 0 || specifyCallCount > 0 || confirmCallCount > 0)
                throw new Exception("Invalid payment sequence.");

            startCallCount++;
        }

        public void SpecifyAmount(double amount)
        {
            if (startCallCount != 1 || specifyCallCount > 0 || confirmCallCount > 0)
                throw new Exception("Invalid payment sequence.");

            if (amount > Balance) 
                throw new InvalidOperationException("Insufficient funds.");

            specifyCallCount++;
            Balance -= amount;
        }

        public void ConfirmPayment()
        {
            if (startCallCount != 1 || specifyCallCount != 1 || confirmCallCount > 0)
                throw new Exception("Invalid payment sequence.");

            confirmCallCount++;
        }

        public bool SuccessFul()
        {
            return startCallCount == 1 && specifyCallCount == 1 && confirmCallCount == 1;
        }


        public void CancelPayment()
        {
            startCallCount = 0;
            specifyCallCount = 0;
            confirmCallCount = 0;
        }
    }
}
