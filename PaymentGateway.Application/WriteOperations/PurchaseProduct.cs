using Abstractions;
using PaymentGateway.Data;
using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.WriteSide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Application.WriteOperations
{
    class PurchaseProduct : IWriteOperation<Command>
    {
        public IEventSender eventSender;
        public PurchaseProduct(IEventSender eventSender)
        {
            this.eventSender = eventSender;
        }

      
        public void PerformOperation(Command operation)
        {
            Database database = Database.GetInstance();

            Transaction transaction = new Transaction();

            BankAccount account = database.BankAccounts.FirstOrDefault(x => x.Iban == operation.Iban);

            if (account == null)
            {
                throw new Exception("Invalid Account");
            }
            double total = 0;
            foreach (var item in operation.Details)
            {
                Product product = database.Products.FirstOrDefault(x => x.ID == item.idProd);

                if (product.Limit < item.Quantity)
                {
                    throw new Exception("Product not in stock");
                }
                total += product.Value * item.Quantity;

                if (account.Balance < total)
                {
                    throw new Exception("Insufficient funds");
                }

                ProductXTransaction pxt = new ProductXTransaction
                {
                    IdProduct = product.ID,
                    IdTransaction = transaction.ID,
                    Quantity = item.Quantity
                };
                product.Limit -= item.Quantity;

               
                database.ProductXTransaction.Add(pxt);
            }


            database.SaveChanges();
        }
    }
}
