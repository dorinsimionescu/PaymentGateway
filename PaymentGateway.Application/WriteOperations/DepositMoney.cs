using Abstractions;
using PaymentGateway.Data;
using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.Events;
using PaymentGateway.PublishedLanguage.WriteSide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Application.WriteOperations
{
    //cont si tranzactie, +valuta
   public class DepositMoney : IWriteOperation<MakeNewDeposit>
    {
        public IEventSender eventSender;
        public DepositMoney(IEventSender eventSender)
        {
            this.eventSender = eventSender;
        }


        public void PerformOperation(MakeNewDeposit operation)
        {
            var database = Database.GetInstance();
            var person = database.Persons.FirstOrDefault(p => p.Cnp == operation.Cnp);
            if (person == null)
            {
                throw new Exception("User Not Found");

            }

            var account = database.BankAccounts.FirstOrDefault(acc => acc.Iban == operation.Iban);
            if (account == null)
            {
                throw new Exception("Account Not Found");
            }

            var transaction = new Transaction();
            transaction.Amount = operation.Amount;
            transaction.Currency = operation.Currency;
            transaction.Date = DateTime.UtcNow;
            transaction.Type = "Depunere";

            account.Balance += operation.Amount;
            database.SaveChanges();

            var dm = new DepositMade();
            dm.Name = person.Name;
            dm.Amount = operation.Amount;
            dm.Iban = operation.Iban;
            eventSender.SendEvent(dm);





        }
    }
}