using Microsoft.AspNetCore.Http;
using Domain.Models;
using Infrastructure.Services.CustomerService;
using MediatR;

namespace Application.Apis.Customer.Queries.GetCustomer
{
    public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, ResponseModel>
    {
        public GetCustomerQueryHandler(ICustomerService customerService)
        {
            CustomerService = customerService;

        }
        public ICustomerService CustomerService { get; set; }
        public  async Task<ResponseModel> Handle(GetCustomerQuery request, CancellationToken _)
        {
            var customer = await CustomerService.GetCustomer(request.Id);

            if (customer==null)
            {
                return new ResponseModel()
                {
                    Data = null,
                    Status = StatusCodes.Status404NotFound,
                    Message = "not found"
                };
            }
            return new ResponseModel()
            {
                Data = customer,
                Message = "success",
                Status = StatusCodes.Status200OK
            };
        }
    }
}