using Domain.Models;
using MediatR;

namespace Application.Apis.Customer.Commands.CreateCustomer
{
    public class CreateCustomerCommand : IRequest<ResponseModel>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string BankAccountNumber { get; set; }
    }
}