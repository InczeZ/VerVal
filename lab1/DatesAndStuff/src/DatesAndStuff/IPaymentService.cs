namespace DatesAndStuff
{
    public interface IPaymentService
    {
        void StartPayment();
        void SpecifyAmount(double amount);
        void ConfirmPayment();
        void CancelPayment();
        bool SuccessFul();

        double Balance { get; set; }

    }
}
