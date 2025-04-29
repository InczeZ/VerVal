namespace DatesAndStuff.Tests
{
    internal class TestPaymentService : IPaymentService
    {
        private uint startCallCount = 0;
        private uint specifyCallCount = 0;
        public double Balance { get; set; }
        private uint confirmCallCount = 0;

        public TestPaymentService(double balance)
        {
            this.Balance = balance;
        }

        public TestPaymentService()
        {
        }


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
