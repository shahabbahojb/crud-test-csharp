
using Domain.Models;
using MediatR;

namespace Application.Apis.Customer.Queries.GetCustomersList
{
    public class GetCustomersListQuery : IRequest<ResponseModel>
    {
    }
}