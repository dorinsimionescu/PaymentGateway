using System;

namespace PaymentGateway.Models
{
    public class Product
    {
        public int Id  { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }

        public string Currency { get; set; }
        public decimal Limit { get; set; }
    }
}