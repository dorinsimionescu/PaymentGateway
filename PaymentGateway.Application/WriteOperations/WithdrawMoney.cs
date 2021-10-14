using Abstractions;
using PaymentGateway.Data;
using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.Events;
using PaymentGateway.PublishedLanguage.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using System.Threading;

namespace PaymentGateway.Application.WriteOperations
{
    public class WithdrawMoney : IRequestHandler<MakeWithdraw>
    {
        public IEventSender eventSender;
        private readonly Database _database;

        public WithdrawMoney(IEventSender eventSender, Database database)
        {
            this.eventSender = eventSender;
            _database = database;
        }

        public Task<Unit> Handle(MakeWithdraw request, CancellationToken cancellationToken)
        {
            var account = _database.BankAccounts.FirstOrDefault(acc => acc.Iban == request.Iban);
            if (account == null)
            {
                throw new Exception("invalid account");

            }

            var user = _database.Persons.FirstOrDefault(pers => pers.Cnp == request.Cnp);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            if (user.Accounts.FindIndex(r => r.Iban == account.Iban) == -1)
            {
                throw new Exception("invalid attempt");
            }
            if (account.Limit < request.Amount)
            {
                throw new Exception("cannot withdraw this amount");
            }
            if (account.Balance < request.Amount)
            {
                throw new Exception("insufficient funds");
            }
            var transaction = new Transaction();
            transaction.Amount = request.Amount;
            transaction.Currency = account.Currency;
            transaction.Date = DateTime.UtcNow;
            transaction.Type = "Withdraw";
            account.Balance -= request.Amount;
            _database.SaveChanges();

            WithdrawMade wm = new WithdrawMade();
            wm.Amount = request.Amount;
            wm.Iban = request.Iban;
            eventSender.SendEvent(wm);
            return Unit.Task;
        }
    }
}



