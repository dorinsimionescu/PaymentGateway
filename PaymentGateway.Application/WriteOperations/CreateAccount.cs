using Abstractions;
using PaymentGateway.Application.ReadOperations;
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
    public class CreateAccount : IWriteOperation<MakeNewAccount>
    {
        public IEventSender eventSender;
        public CreateAccount(IEventSender eventSender)
        {
            this.eventSender = eventSender;
        }


        public void PerformOperation(MakeNewAccount operation)
        {
            var random = new Random();

            var database = Database.GetInstance();


            var user = database.Persons?.First(e => e.Cnp == operation.UniqueIdentifier);
            if (user == null)
            {
                throw new Exception("User invalid");
            }



            //database.Persons.Remove(user);
            var account = new BankAccount();
            account.Type = operation.AccountType;
            account.Currency = operation.Valuta;
            account.Balance = 0;
            account.Iban = NewIban.GetNewIban();
            account.Limit = 200;

            database.BankAccounts.Add(account);
            user.Accounts.Add(account);
            database.SaveChanges();
            //database.Persons.Add(user);

            AccountMade ec = new AccountMade();

            ec.Name = user.Name;
            eventSender.SendEvent(ec);







        }

    }
}
