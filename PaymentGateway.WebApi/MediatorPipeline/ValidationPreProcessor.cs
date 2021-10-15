using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using MediatR.Pipeline;

namespace PaymentGateway.WebApi.MediatorPipeline
{
    public class ValidationPreProcessor<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationPreProcessor(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);
            
            //var results = new List<ValidationResult>();
            //var errors = new List<ValidationFailure>();
            //foreach (var validator in _validators)
            //{
            //    var result = validator.Validate(context);
            //    results.Add(result);
            //    if (result.Errors != null)
            //    {
            //        errors.AddRange(result.Errors);
            //    }
            //}

            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0) // errors
            {
                throw new ValidationException(failures);
            }

            return Task.CompletedTask;
        }
    }
}
