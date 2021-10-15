using MediatR;

namespace PaymentGateway.PublishedLanguage.Events
{
   public class DepositMade : INotification
    {
        public string Iban { get; set; }
        public decimal Amount { get; set; }
        public string Name { get; set; }
    }
}
