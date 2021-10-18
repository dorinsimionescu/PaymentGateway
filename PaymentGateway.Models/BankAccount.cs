using System;
using System.Collections.Generic;

#nullable disable

namespace PaymentGateway.Models
{
    public partial class BankAccount
    {
        public BankAccount()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public int PersonId { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; }
        public string Iban { get; set; }
        public string Status { get; set; }
        public decimal? Limit { get; set; }
        public string Type { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
