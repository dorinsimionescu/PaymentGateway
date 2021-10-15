using MediatR;
using PaymentGateway.Application.Services;
using PaymentGateway.Data;
using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.Commands;
using PaymentGateway.PublishedLanguage.Events;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGateway.Application.WriteOperations
{
    public class CreateAccount : IRequestHandler<MakeNewAccount>
    {
        private readonly IMediator _mediator;
        private readonly AccountOptions _accountOptions;
        private readonly PaymentDbContext _dbContext;
        private readonly NewIban _ibanService;

        public CreateAccount(IMediator mediator, AccountOptions accountOptions, PaymentDbContext dbContext, NewIban ibanService)
        {
            _mediator = mediator;
            _accountOptions = accountOptions;
            _dbContext = dbContext;
            _ibanService = ibanService;
        }

        public async Task<Unit> Handle(MakeNewAccount request, CancellationToken cancellationToken)
        {
            var user = _dbContext.Persons.FirstOrDefault(e => e.Cnp == request.UniqueIdentifier);
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
                Limit = 200,
                Status = "Active"
            };

            _dbContext.BankAccounts.Add(account);
            user.Accounts.Add(account);
            _dbContext.SaveChanges();

            AccountMade ec = new AccountMade
            {
                Name = user.Name
            };
            await _mediator.Publish(ec, cancellationToken);
            return Unit.Value;
        }        
    }
}