using AutoMapper;
using MediatR;
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
    //cont si tranzactie, +valuta
    public class DepositMoney : IRequestHandler<MakeNewDeposit>
    {
        private readonly IMediator _mediator;
        private readonly PaymentDbContext _dbContext;
        private readonly IMapper _mapper;
        public DepositMoney(IMediator mediator, PaymentDbContext dbContext, IMapper mapper)
        {
            _mediator = mediator;
            _dbContext = dbContext;
            _mapper = mapper;
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

            var transaction = new Transaction
            {
                Amount = request.Amount,
                Currency = request.Currency,
                Date = DateTime.UtcNow,
                Type = "Depunere"
            };

            account.Balance += request.Amount;
            _dbContext.SaveChanges();

            // if DepositMade has 10 fields, constructing it can be time consuming
            //var dm = new DepositMade
            //{
            //    Name = person.Name,
            //    Amount = request.Amount,
            //    Iban = request.Iban
            //};
            // automapper is working based on same fields name
            var dm = _mapper.Map<DepositMade>(request);

            await _mediator.Publish(dm, cancellationToken);
            return Unit.Value;
        }
    }
}