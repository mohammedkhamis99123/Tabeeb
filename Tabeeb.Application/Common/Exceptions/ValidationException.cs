using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tabeeb.Application.Common.Exceptions
{
    public  class ValidationException():Exception("One or more Validation failures have occurred.")
    {

        public Dictionary<string, string[]>Errors= new Dictionary<string, string[]>();

        public ValidationException(List<ValidationFailure> validationFailures)
            :this()
        {

            Errors = validationFailures
                .GroupBy(a => a.PropertyName, e => e.ErrorMessage)
                .ToDictionary(a => a.Key, e => e.ToArray());

        }
    }
}
