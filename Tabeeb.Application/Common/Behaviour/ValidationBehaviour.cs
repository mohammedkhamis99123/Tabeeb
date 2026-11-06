using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Exceptions;
using ValidationException = Tabeeb.Application.Common.Exceptions.ValidationException;

namespace Tabeeb.Application.Common.Behaviour
{
    public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
       : IPipelineBehavior<TRequest, TResponse>
       where TRequest : notnull
    {
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken
        )
        {
            if (validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var ValidationResults = await Task.WhenAll(
                    validators.Select(v => v.ValidateAsync(context, cancellationToken))
                );

                var failures = ValidationResults.Where(r => r.Errors.Any()).SelectMany(r => r.Errors).ToList();

                // If failures are found, modify the error key if necessary
                if (failures.Any())
                {
                    foreach (var failure in failures)
                    {
                        // Check if the failure PropertyName is empty (which happens when using anonymous objects)
                        if (string.IsNullOrWhiteSpace(failure.PropertyName))
                        {
                            // Set the error key to "otherErrors"
                            failure.PropertyName = "otherErrors";
                            continue;
                        }

                        failure.PropertyName = failure.PropertyName;
                    }

                    throw new ValidationException(failures);
                }
            }
            return await next();
        }
    }
}
