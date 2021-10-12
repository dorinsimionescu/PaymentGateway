using Abstractions;
using PaymentGateway.Data;
using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.Events;
using PaymentGateway.PublishedLanguage.WriteSide;
using System;
using System.Linq;

namespace PaymentGateway.Application.WriteOperations
{
    //cont si tranzactie, +valuta
    public class DepositMoney : IWriteOperation<MakeNewDeposit>
    {
        private readonly IEventSender _eventSender;
        private readonly Database _database;
        public DepositMoney(IEventSender eventSender, Database database)
        {
            _eventSender = eventSender;
            _database = database;
        }

        public void PerformOperation(MakeNewDeposit operation)
        {
            var person = _database.Persons.FirstOrDefault(p => p.Cnp == operation.Cnp);
            if (person == null)
            {
                throw new Exception("User Not Found");
            }

            var account = _database.BankAccounts.FirstOrDefault(acc => acc.Iban == operation.Iban);
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
            _database.SaveChanges();

            var dm = new DepositMade();
            dm.Name = person.Name;
            dm.Amount = operation.Amount;
            dm.Iban = operation.Iban;
            _eventSender.SendEvent(dm);
        }
    }
}