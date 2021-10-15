using MediatR;

namespace PaymentGateway.PublishedLanguage.Commands
{
    public class MakeNewDeposit: IRequest
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        
        public string Iban { get; set; }

        public string Cnp { get; set; }
    }
}
