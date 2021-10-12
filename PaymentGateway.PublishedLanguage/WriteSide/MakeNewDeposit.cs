using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.PublishedLanguage.WriteSide
{
   public class MakeNewDeposit
    {
        public double Amount { get; set; }
        public string Currency { get; set; }
        
        public string Iban { get; set; }

        public string Cnp { get; set; }
    }
}
