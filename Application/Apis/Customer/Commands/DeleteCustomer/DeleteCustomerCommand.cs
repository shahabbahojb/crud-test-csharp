using Domain.Models;
using MediatR;

namespace Application.Apis.Customer.Commands.DeleteCustomer
{
    public class DeleteCustomerCommand : IRequest<ResponseModel>
    {
        public int Id { get; set; }
    }
}