using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstractions;
using PaymentGateway.Application.ReadOperations;
using PaymentGateway.Data;
using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.WriteSide;

namespace PaymentGateway.Application.WriteOperations
{
    public class EnrollCustomerOperation : IWriteOperation<EnrollCustomer>
    {
        public IEventSender eventSender;
        public EnrollCustomerOperation(IEventSender eventSender)
        {
            this.eventSender = eventSender;
        }

        public void PerformOperation(EnrollCustomer operation)
        {
            var random = new Random();

            var database = Database.GetInstance();

            var customer = new Person();
            

            customer.Cnp = operation.UniqueIdentifier;
            customer.Name = operation.Name;
            if(operation.ClientType=="Company")
            {
                customer.TypeOfPerson = PersonType.Company;
            }

            else if (operation.ClientType == "Individual")
            {
                customer.TypeOfPerson = PersonType.Individual;
            }
            else
            {
                throw new Exception("Unsupported person type");
            }

            database.Persons.Add(customer);

            var account = new BankAccount();
            account.Type = operation.AccountType;
            account.Currency = operation.Valuta;
            account.Balance = 0;
            account.Iban = NewIban.GetNewIban();

            database.BankAccounts.Add(account);

            database.SaveChanges();

            EnrollCustomer ec = new EnrollCustomer();
            ec.Name = customer.Name;
            ec.UniqueIdentifier = customer.Cnp;
            ec.ClientType = operation.ClientType;
            eventSender.SendEvent(ec);


        }
    }
}
