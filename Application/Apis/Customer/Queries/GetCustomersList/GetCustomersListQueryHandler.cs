using Microsoft.AspNetCore.Http;
using Domain.Models;
using Infrastructure.Services.CustomerService;
using MediatR;

namespace Application.Apis.Customer.Queries.GetCustomersList
{
    public class GetCustomersListQueryHandler : IRequestHandler<GetCustomersListQuery, ResponseModel>
    {
        public ICustomerService CustomerService { get; set; }
        public GetCustomersListQueryHandler(ICustomerService customerService)
        {
            CustomerService = customerService;
        }

        public  async Task<ResponseModel> Handle(GetCustomersListQuery request, CancellationToken _)
        {
            return new ResponseModel()
            {
                Data =await CustomerService.GetCustomersList(),
                Message = "success",
                Status = StatusCodes.Status200OK
            };
        }
    }
}