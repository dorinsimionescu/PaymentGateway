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
using AutoMapper;

namespace PaymentGateway.Application.WriteOperations
{
    public class WithdrawMoney : IRequestHandler<MakeWithdraw>
    {
        private readonly IMediator _mediator;
        private readonly PaymentDbContext _dbContext;
        private readonly IMapper _mapper;

        public WithdrawMoney(IMediator mediator, PaymentDbContext dbContext, IMapper mapper)
        {
            _mediator = mediator;
            _dbContext = dbContext;
            _mapper = mapper;
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
            // todo: refactor
            //if (user.Accounts.FindIndex(r => r.Iban == account.Iban) == -1)
            //{
            //    throw new Exception("invalid attempt");
            //}
            if (account.Limit < request.Amount)
            {
                throw new Exception("cannot withdraw this amount");
            }
            if (account.Balance < request.Amount)
            {
                throw new Exception("insufficient funds");
            }
            var transaction = new Transaction
            {
                Amount = request.Amount,
                Currency = account.Currency,
                Date = DateTime.UtcNow,
                Type = "Withdraw"
            };
            account.Balance -= request.Amount;
            _dbContext.SaveChanges();

            var wm = _mapper.Map<WithdrawMade>(request);
            //var wm = new WithdrawMade
            //{
            //    Amount = request.Amount,
            //    Iban = request.Iban
            //};
            await _mediator.Publish(wm, cancellationToken);
            return Unit.Value;
        }
    }
}