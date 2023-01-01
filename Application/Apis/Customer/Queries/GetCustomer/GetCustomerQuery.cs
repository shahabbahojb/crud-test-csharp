
using Domain.Models;
using MediatR;

namespace Application.Apis.Customer.Queries.GetCustomer
{
    public class GetCustomerQuery : IRequest<ResponseModel>
    {
        public int  Id { get; set; }
    }
}