using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.PublishedLanguage.Events
{
   public class DepositMade
    {
        public string Iban { get; set; }
        public double Amount { get; set; }
        public string Name { get; set; }
    }
}
