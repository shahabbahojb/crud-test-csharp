using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Validation
{
    public abstract class Validator<T, TC> : AbstractValidator<T> where TC : DbContext
    {
        public static string Default = "درخواست معتبر نیست.";

        protected Validator(TC context = null)
        {
            Context = context;
        }

        protected TC Context { get; set; }



        public ValidationResult StdValidate(ValidationContext<T> context)
        {
            return Validate(context);
        }

  

        public ValidationResult StdValidate(T instance)
        {
            return ValidationsTools.FromFluentValidationResult(Validate(instance));
        }

        public Task<ValidationResult> StdValidateAsync(T instance, CancellationToken cancellation = default)
        {
            return ValidateAsync(instance, cancellation)
                .ContinueWith(x => {
                    return ValidationsTools.FromFluentValidationResult(x.Result);
                },cancellation);
        }
    }
}