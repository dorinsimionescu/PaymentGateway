using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Models
{
    public class Product
    {
        public int ID  { get; set; } 
        public string Name { get; set; }
        public double Value { get; set; }

        public String Currency { get; set; }

        public int Limit { get; set; }


    }
}
