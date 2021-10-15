using Abstractions;
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
    //cont si tranzactie, +valuta
    public class DepositMoney : IRequestHandler<MakeNewDeposit>
    {
        private readonly IMediator _mediator;
        private readonly PaymentDbContext _dbContext;
        public DepositMoney(IMediator mediator, PaymentDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(MakeNewDeposit request, CancellationToken cancellationToken)
        {
            var person = _dbContext.Persons.FirstOrDefault(p => p.Cnp == request.Cnp);
            if (person == null)
            {
                throw new Exception("User Not Found");
            }

            var account = _dbContext.BankAccounts.FirstOrDefault(acc => acc.Iban == request.Iban);
            if (account == null)
            {
                throw new Exception("Account Not Found");
            }

            var transaction = new Transaction();
            transaction.Amount = request.Amount;
            transaction.Currency = request.Currency;
            transaction.Date = DateTime.UtcNow;
            transaction.Type = "Depunere";

            account.Balance += request.Amount;
            _dbContext.SaveChanges();

            var dm = new DepositMade();
            dm.Name = person.Name;
            dm.Amount = request.Amount;
            dm.Iban = request.Iban;
            await _mediator.Publish(dm, cancellationToken);
            return Unit.Value;
        }
    }
}