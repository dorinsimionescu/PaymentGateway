using Abstractions;
using MediatR;
using PaymentGateway.Data;
using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.Commands;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGateway.Application.WriteOperations
{
    public class PurchaseProduct : IRequestHandler<Command>
    {
        private readonly IMediator _mediator;
        private readonly PaymentDbContext _dbContext;

        public PurchaseProduct(IMediator mediator, PaymentDbContext dbContext)
        {
            _dbContext = dbContext;
            _mediator = mediator;
        }

        public Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {

            var account = _dbContext.BankAccounts.FirstOrDefault(x => x.Iban == request.Iban);

            if (account == null)
            {
                throw new Exception("Invalid Account");
            }

            decimal total = 0;

            var transaction = new Transaction
            {
                Currency = account.Currency,
                Date = DateTime.UtcNow,
                Type = "Purchase",
                AccountId = account.Id
            };
            _dbContext.Transactions.Add(transaction);
            _dbContext.SaveChanges();
            foreach (var item in request.Details)
            {
                Product product = _dbContext.Products.FirstOrDefault(x => x.Id == item.ProductId);

                if (product.Limit < item.Quantity)
                {
                    throw new Exception("Product not in stock");
                }
                total += product.Value * item.Quantity;

                if (account.Balance < total)
                {
                    throw new Exception("Insufficient funds");
                }

                var pxt = new ProductXtransaction
                {
                    IdProduct = product.Id,
                    IdTransaction = transaction.Id,
                    Quantity = item.Quantity
                };
                product.Limit -= item.Quantity;


                _dbContext.ProductXtransactions.Add(pxt);
            }
            transaction.Amount = total;

            _dbContext.SaveChanges();
            return Unit.Task;
        }
    }
}
