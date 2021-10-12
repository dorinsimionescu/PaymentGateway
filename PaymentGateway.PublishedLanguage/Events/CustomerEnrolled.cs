using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.PublishedLanguage.Events
{
   public  class CustomerEnrolled
    {
        public string Name { get; set; }
        public int UniqueIdentifier { get; set; }
        public string ClientType { get; set; }


    }
}
