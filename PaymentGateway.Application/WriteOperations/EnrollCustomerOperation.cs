using Abstractions;
using PaymentGateway.Application.Services;
using PaymentGateway.Data;
using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.WriteSide;
using System;

namespace PaymentGateway.Application.WriteOperations
{
    public class EnrollCustomerOperation : IWriteOperation<EnrollCustomer>
    {
        private readonly IEventSender _eventSender;
        private readonly Database _database;
        private readonly NewIban _ibanService;
        public EnrollCustomerOperation(IEventSender eventSender, Database database, NewIban ibanService)
        {
            _eventSender = eventSender;
            _database = database;
            _ibanService = ibanService;
        }

        public void PerformOperation(EnrollCustomer operation)
        {
            var customer = new Person
            {
                Cnp = operation.UniqueIdentifier,
                Name = operation.Name
            };
            if (operation.ClientType == "Company")
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

            customer.Id = _database.Persons.Count + 1;
            _database.Persons.Add(customer);

            var account = new BankAccount();
            account.Type = operation.AccountType;
            account.Currency = operation.Valuta;
            account.Balance = 0;
            account.Iban = _ibanService.GetNewIban();

            _database.BankAccounts.Add(account);

            _database.SaveChanges();

            EnrollCustomer ec = new EnrollCustomer();
            ec.Name = customer.Name;
            ec.UniqueIdentifier = customer.Cnp;
            ec.ClientType = operation.ClientType;
            _eventSender.SendEvent(ec);

        }
    }
}