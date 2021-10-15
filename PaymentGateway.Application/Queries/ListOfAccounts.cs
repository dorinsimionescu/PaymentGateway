using Abstractions;
using FluentValidation;
using MediatR;
using PaymentGateway.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGateway.Application.Queries
{
    public class ListOfAccounts
    {
        public class Validator : AbstractValidator<Query>
        {
            public Validator(PaymentDbContext _dbContext)
            {
                RuleFor(q => q).Must(query =>
                {
                    var person = query.PersonId.HasValue ?
                    _dbContext.Persons.FirstOrDefault(x => x.Id == query.PersonId) :
                    _dbContext.Persons.FirstOrDefault(x => x.Cnp == query.Cnp);

                    return person != null;
                }).WithMessage("Customer not found");
            }
        }

        public class Validator2 : AbstractValidator<Query>
        {
            public Validator2(PaymentDbContext dbContext)
            {
                RuleFor(q => q).Must(q=>
                {
                    return q.PersonId.HasValue || !string.IsNullOrEmpty(q.Cnp);
                }).WithMessage("Customer data is invalid");

                RuleFor(q => q.Cnp).Must(cnp=>
                {
                    if (string.IsNullOrEmpty(cnp))
                    {
                        return true;
                    }
                    return cnp.Length == 13;
                }).WithMessage("CNP has wrong lenght. Expected 13");

                RuleFor(q => q.PersonId).Must(personId =>
                {
                    if (!personId.HasValue)
                    {
                        return true;
                    }
                    return personId.Value > 0;
                }).WithMessage("Person id is not positive");
            }
        }

        public class Query : IRequest<List<Model>>
        {
            public int? PersonId { get; set; }
            public string Cnp { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, List<Model>>
        {
            private readonly PaymentDbContext _dbContext;

            public QueryHandler(PaymentDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public Task<List<Model>> Handle(Query request, CancellationToken cancellationToken)
            {
                var person = request.PersonId.HasValue ?
                   _dbContext.Persons.FirstOrDefault(x => x.Id == request.PersonId) :
                   _dbContext.Persons.FirstOrDefault(x => x.Cnp == request.Cnp);

                /*
                _dbContext.Persons.Where(x => x.Name.Contains("Vasile")); // select * from persons where name like '%Vasile%'
                _dbContext.Persons.FirstOrDefault(x => x.Name.Contains("Vasile")); // select top 1 * from persons where name like '%Vasile%'
                _database
                    .Persons
                    .Where(x => x.Name.Contains("Vasile"))
                    .Select(x => new { x.Name, x.Cnp })
                    .ToList(); //  select name, cnp from persons where name like '%Vasile%'

                _database
                    .Persons
                    .Where(x => x.Name.Contains("Vasile"))
                    .Take(5)
                    .OrderBy(x=> x.Cnp)
                    ; // select top 5 * from persons where name like '%Vasile%' order by cnp

                _database
                    .Persons
                    .Where(x => x.Name.Contains("Vasile"))
                    .Skip(10)
                    .Take(5)
                    //.OrderBy(x => x.Cnp)
                    ; // select * from persons where name like '%Vasile%' limit 5, offset 10 order by cnp -- ia randurile de la 11 la 15 ordonate dupa CNP. 

                _database
                    .Persons
                    .Where(x => x.Name.Contains("Vasile"))
                    .Skip(10)
                    .Take(5)
                    ; // ia randurile de la 11 la 15 ordonate dupa CNP. 
                */

                var db = _dbContext.BankAccounts.Where(x => x.PersonId == person.Id);
                var result = db.Select(x => new Model
                {
                    Balance = x.Balance,
                    Currency = x.Currency,
                    Iban = x.Iban,
                    Id = x.Id,
                    Limit = x.Limit,
                    Status = x.Status,
                    Type = x.Type
                }).ToList();
                return Task.FromResult(result);
            }
        }

        public class Model
        {
            public int Id { get; set; }
            public decimal Balance { get; set; }
            public string Currency { get; set; }
            public string Iban { get; set; }
            public string Status { get; set; }
            public decimal Limit { get; set; }
            public string Type { get; set; }
        }
    }
}
