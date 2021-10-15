﻿using Abstractions;
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
        private readonly IMediator _mediator;
        private readonly PaymentDbContext _dbContext;

        public WithdrawMoney(IMediator mediator, PaymentDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(MakeWithdraw request, CancellationToken cancellationToken)
        {
            var account = _dbContext.BankAccounts.FirstOrDefault(acc => acc.Iban == request.Iban);
            if (account == null)
            {
                throw new Exception("invalid account");

            }

            var user = _dbContext.Persons.FirstOrDefault(pers => pers.Cnp == request.Cnp);
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
            _dbContext.SaveChanges();

            WithdrawMade wm = new WithdrawMade
            {
                Amount = request.Amount,
                Iban = request.Iban
            };
            await _mediator.Publish(wm, cancellationToken);
            return Unit.Value;
        }
    }
}