using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.PublishedLanguage.Events
{
   public class WithdrawMade
    {
        public string Iban { get; set; }
        public double Amount { get; set; }
    }
}
