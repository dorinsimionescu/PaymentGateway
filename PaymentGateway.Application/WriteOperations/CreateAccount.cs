using Abstractions;
using PaymentGateway.Application.ReadOperations;
using PaymentGateway.Application.Services;
using PaymentGateway.Data;
using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.Events;
using PaymentGateway.PublishedLanguage.Commands;
using System;
using System.Linq;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

namespace PaymentGateway.Application.WriteOperations
{
    public class CreateAccount : IRequestHandler<MakeNewAccount>
    {
        private readonly IEventSender _eventSender;
        private readonly AccountOptions _accountOptions;
        private readonly Database _database;
        private readonly NewIban _ibanService;

        public CreateAccount(IEventSender eventSender, AccountOptions accountOptions, Database database, NewIban ibanService)
        {
            _eventSender = eventSender;
            _accountOptions = accountOptions;
            _database = database;
            _ibanService = ibanService;
        }

        public Task<Unit> Handle(MakeNewAccount request, CancellationToken cancellationToken)
        {
            var user = _database.Persons.FirstOrDefault(e => e.Cnp == request.UniqueIdentifier);
            if (user == null)
            {
                throw new Exception("User invalid");
            }

            var account = new BankAccount
            {
                Type = request.AccountType,
                Currency = request.Valuta,
                Balance = _accountOptions.InitialBalance,
                Iban = _ibanService.GetNewIban(),
                Limit = 200
            };

            _database.BankAccounts.Add(account);
            user.Accounts.Add(account);
            _database.SaveChanges();

            AccountMade ec = new AccountMade
            {
                Name = user.Name
            };
            _eventSender.SendEvent(ec);
            return Unit.Task;
        }        
    }
}