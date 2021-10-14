using Abstractions;
using MediatR;
using PaymentGateway.Application.Services;
using PaymentGateway.Data;
using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGateway.Application.WriteOperations
{
    public class EnrollCustomerOperation : IRequestHandler<EnrollCustomer>
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

        public Task<Unit> Handle(EnrollCustomer request, CancellationToken cancellationToken)
        {
            var customer = new Person
            {
                Cnp = request.UniqueIdentifier,
                Name = request.Name
            };
            if (request.ClientType == "Company")
            {
                customer.TypeOfPerson = PersonType.Company;
            }

            else if (request.ClientType == "Individual")
            {
                customer.TypeOfPerson = PersonType.Individual;
            }
            else
            {
                throw new Exception("Unsupported person type");
            }

            customer.Id = _database.Persons.Count + 1;
            _database.Persons.Add(customer);

            var account = new BankAccount
            {
                Type = request.AccountType,
                Currency = request.Valuta,
                Balance = 0,
                Iban = _ibanService.GetNewIban()
            };

            _database.BankAccounts.Add(account);

            _database.SaveChanges();

            EnrollCustomer ec = new EnrollCustomer
            {
                Name = customer.Name,
                UniqueIdentifier = customer.Cnp,
                ClientType = request.ClientType
            };
            _eventSender.SendEvent(ec);
            return Unit.Task;
        }
    }
}