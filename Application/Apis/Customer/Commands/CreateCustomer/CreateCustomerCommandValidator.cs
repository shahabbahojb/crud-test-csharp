using Application.Common.Validation;
using FluentValidation;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using PhoneNumbers;

namespace Application.Apis.Customer.Commands.CreateCustomer
{
    public class CreateCustomerCommandValidator : Validator<CreateCustomerCommand, AppDbContext>
    {
        public CreateCustomerCommandValidator(AppDbContext context) : base(context)
        {
            PhoneNumber();
            Email();
            BankAccountNumber();
            FirstName();
            LastName();
            DateOfBirth();
        }

        private void FirstName()
        {
            RuleFor(x => x.FirstName)
                .MustAsync(async (x,_)=> !await Context.Customers.AnyAsync(y=>y.FirstName==x,CancellationToken.None))
                .WhenNotNull()
                .WithMessage("FirstName is not unique");
        }
        
        private void LastName()
        {
            RuleFor(x => x.LastName)
                .MustAsync(async (x,_)=> !await Context.Customers.AnyAsync(y=>y.LastName==x,CancellationToken.None))
                .WhenNotNull()
                .WithMessage("LastName is not unique");
        }
        
        private void DateOfBirth()
        {
            RuleFor(x => x.DateOfBirth)
                .MustAsync(async (x,_)=> !await Context.Customers.AnyAsync(y=>y.DateOfBirth==x,CancellationToken.None))
                .WhenNotNull()
                .WithMessage("DateOfBirth is not unique");
        }

        private void BankAccountNumber()
        {
            RuleFor(x => x.BankAccountNumber)
                .Matches("((\\d{4})-){3}\\d{4}")
                .WhenNotNull()
                .WithMessage("BankAccountNumber is not valid");
        }

        private void Email()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .WhenNotNull()
                .WithMessage("Email is not valid");
        }

        private void PhoneNumber()
        {
            RuleFor(x => x.PhoneNumber).MustAsync(async (x,_)=> PhoneNumberUtil.IsViablePhoneNumber(x))
                .WhenNotNull()
                .WithMessage("PhoneNumber is not valid");
        }
    }
}