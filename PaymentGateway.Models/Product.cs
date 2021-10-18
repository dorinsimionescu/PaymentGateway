using System;
using System.Collections.Generic;

#nullable disable

namespace PaymentGateway.Models
{
    public partial class Product
    {
        public Product()
        {
            ProductXtransactions = new HashSet<ProductXtransaction>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public string Currency { get; set; }
        public decimal? Limit { get; set; }

        public virtual ICollection<ProductXtransaction> ProductXtransactions { get; set; }
    }
}
