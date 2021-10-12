using Abstractions;
using System;

namespace PaymentGateway.ExternalService
{
    public class EventSender : IEventSender
    {
        public void SendEvent(object obj)
        {
            Console.WriteLine(obj);
        }
    }
}
