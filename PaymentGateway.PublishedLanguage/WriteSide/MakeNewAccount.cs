using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.PublishedLanguage.WriteSide
{
    public class MakeNewAccount
    {
        public string UniqueIdentifier { get; set; }
        public string AccountType { get; set; }
        public string Valuta { get; set; }
    }
}
