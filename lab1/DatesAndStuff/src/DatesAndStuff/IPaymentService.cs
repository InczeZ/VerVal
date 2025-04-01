using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatesAndStuff
{
    public interface IPaymentService
    {
        void StartPayment();
        void SpecifyAmount(double amount);
        void ConfirmPayment();
        void CancelPayment();
        bool SuccessFul();

        double Balance { get; }
    }
}
