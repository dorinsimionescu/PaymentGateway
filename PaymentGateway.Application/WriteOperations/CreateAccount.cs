using Abstractions;
using PaymentGateway.Application.ReadOperations;
using PaymentGateway.Data;
using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.Events;
using PaymentGateway.PublishedLanguage.WriteSide;
using System;
using System.Linq;

namespace PaymentGateway.Application.WriteOperations
{
    public class CreateAccount : IWriteOperation<MakeNewAccount>
    {
        private readonly IEventSender _eventSender;
        private readonly AccountOptions _accountOptions;

        public CreateAccount(IEventSender eventSender, AccountOptions accountOptions)
        {
            _eventSender = eventSender;
            _accountOptions = accountOptions;
        }

        public void PerformOperation(MakeNewAccount operation)
        {
            var database = Database.GetInstance();

            var user = database.Persons.FirstOrDefault(e => e.Cnp == operation.UniqueIdentifier);
            if (user == null)
            {
                throw new Exception("User invalid");
            }

            var account = new BankAccount
            {
                Type = operation.AccountType,
                Currency = operation.Valuta,
                Balance = _accountOptions.InitialBalance,
                Iban = NewIban.GetNewIban(),
                Limit = 200
            };

            database.BankAccounts.Add(account);
            user.Accounts.Add(account);
            database.SaveChanges();

            AccountMade ec = new AccountMade
            {
                Name = user.Name
            };
            _eventSender.SendEvent(ec);
        }
    }
}