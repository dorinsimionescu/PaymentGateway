using MediatR;

namespace PaymentGateway.PublishedLanguage.Events
{
   public  class CustomerEnrolled : INotification
    {
        public string Name { get; set; }
        public int UniqueIdentifier { get; set; }
        public string ClientType { get; set; }

    }
}